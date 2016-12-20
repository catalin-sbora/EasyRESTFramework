
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Internal;
using EasyRESTFramework.Client.Abstractions;
using System.Reflection;
using EasyRESTFramework.Client.Filters;

namespace EasyRESTFramework.Client
{
    public class WsSet<TEntity> : IWsSet<TEntity> where TEntity: WsObject
    {
        private readonly IWsContext _wsContext;
        private readonly Dictionary<TEntity, TEntity> _entities = new Dictionary<TEntity, TEntity>();
        private readonly ObjectStateManager _stateManager = new ObjectStateManager();
        //private readonly HashSet<EntityContainer> _collection = new HashSet<EntityContainer>();
        
        internal WsSet(IWsContext wsContext, ObjectStateManager stateManager)
        {
            this._wsContext = wsContext;
        }

        public WsSet(IWsContext wsContext): this(wsContext, null)
        {
           
        }

        private void ShallowCopy(TEntity source, TEntity destination)
        {
            var properties = source.GetType().GetRuntimeProperties().Where(property => property.CanWrite == true );
            foreach (PropertyInfo property in properties)
            {
                var fieldValue = property.GetValue(source);
                property.SetValue(destination, fieldValue);
            }
        }

        public bool Add(TEntity toAdd)
        {
            bool shouldAdd = true;
            bool addResult = false;
            if (_stateManager != null)
            {
                shouldAdd = _stateManager.AddObject(toAdd);
            }
            if (shouldAdd)
            {
                if (!_entities.ContainsKey(toAdd))
                {
                    _entities.Add(toAdd, toAdd);
                    addResult = true;
                }
                else
                {
                    addResult = false;
                }   
            }

            return addResult;
        }

        public bool Remove(TEntity toRemove)
        {
            bool shouldRemove = true;
            bool result = false;
            if (_stateManager != null)
            {
                shouldRemove = _stateManager.DeleteObject(toRemove);
            }
            if (shouldRemove)
            {
                result = _entities.Remove(toRemove);
            }
            return result;
        }

        public bool Update(TEntity toUpdate)
        {
            bool shouldUpdate = true;
            bool result = false;
            if (_stateManager != null)
            {
                shouldUpdate = _stateManager.UpdateObject(toUpdate);
            }
            if (shouldUpdate)
            {
                if (_entities.ContainsKey(toUpdate))
                {
                    ShallowCopy(toUpdate, _entities[toUpdate]);
                    result = true;
                }
            }

            return result;
        }       
        
        public IEnumerable<TEntity> GetAll()
        {
            //get the infor directly from the server. Consider implementing caching for handling offline sessions
            //return _wsContext.RESTClient.GetItems<TEntity>();
            IEnumerable<TEntity> result = GetFilteredData(null);

            return result;
        }

        public IEnumerable<TEntity> GetFilteredData(QueryFilter filter)
        {
            IEnumerable<TEntity> result = null;
                result = GetFilteredDataAsync(filter).Result;
            return result;
        }

        
        public Type GetStoredType()
        {
            return typeof(TEntity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var result = await GetFilteredDataAsync(null);
            return result;
        }

        public async Task<IEnumerable<TEntity>> GetFilteredDataAsync(QueryFilter filter)
        {
            var result = new List<TEntity>();
            bool failedToGetData = false;
            try
            {
                IEnumerable<TEntity> internalResult = null;
                internalResult =  await _wsContext.RESTClient.GetItemsAsync<TEntity>(filter);
                Dictionary<TEntity, TEntity> tempResult = new Dictionary<TEntity, TEntity>();
                foreach (TEntity item in internalResult)
                {
                    if (_entities.ContainsKey(item))
                    {
                        ShallowCopy(_entities[item], item);
                        tempResult[item] = item;
                        result.Add(_entities[item]);
                    }
                    else
                    {

                        //add this to our _entities list
                        _entities.Add(item, item);
                        result.Add(item);
                        tempResult.Add(item, item);
                    }
                }

                //now we should remove any entity that is on our side but is not on the server side                
                if (_entities.Count > internalResult.Count())
                {
                    var toRemove = new List<TEntity>();
                    foreach (TEntity entity in _entities.Keys)
                    {
                        if (!tempResult.ContainsKey(entity))
                        {
                            toRemove.Add(entity);
                        }
                    }

                    foreach (TEntity entity in toRemove)
                    {
                        _entities.Remove(entity);
                    }
                }
            }
            catch (Exception)
            {
                failedToGetData = true;
            }

            if (failedToGetData)
            {
                //use what we have in memory
                if (filter == null)
                {
                    result = _entities.Values.ToList();
                }
                else
                {
                    result = _entities.Values.Where(_wsContext.FilterBuilder.CreateExpressionFilter<TEntity>(filter)).ToList();
                }
            }
            return result;
        }
    }
}
