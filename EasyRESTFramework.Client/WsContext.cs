using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Abstractions;
using EasyRESTFramework.Client.Internal;
using System.Reflection;

namespace EasyRESTFramework.Client
{
    public class WsContext: IWsContext
    {
        //private Dictionary<string, IStateAccessor> dataSets = new Dictionary<string, IStateAccessor>();
        private readonly Dictionary<String, ObjectStateManager> _setStatesManagers = new Dictionary<string, ObjectStateManager>();
        private readonly Dictionary<String, object> _availableSets = new Dictionary<string, object>();        
        private readonly IRestClient _restClient;

        
        public WsContext(IRestClient restClient)
        {
            _restClient = restClient;            
        }

        private async Task SaveAddedObjects()
        {
            foreach (var stateManagerEntry in _setStatesManagers)
            {
                IEnumerable<WsObject> added = stateManagerEntry.Value.GetAddedObjects();
                if (added.Count<WsObject>() > 0)
                {
                    //save added
                    if (_restClient != null)
                    {
                        IEnumerable<WsObject> postedItems =  await _restClient.PostItemsAsync(added);
                        var savedItemsCount = postedItems.Count();
                        var addedItemsCount = added.Count();
                        if (savedItemsCount == addedItemsCount)
                        {
                            for (int i = 0; i < savedItemsCount; i++)
                            {
                                added.ElementAt(i).Id = postedItems.ElementAt(i).Id;
                            }
                        }
                        else
                        {
                            throw new Exception("Not all the elements were saved on the server side. We received less elements than we sent.");
                            //some data is missing and could not be saved.
                            //the entire data set needs to be reloaded to see what has been saved

                        } 
                    }
                }
            }

        }
        private async Task SaveModifedObjects()
        {
            foreach (var stateManagerEntry in _setStatesManagers)
            {
                IEnumerable<WsObject> modified = stateManagerEntry.Value.GetModifiedObjects();
                if (modified.Count<WsObject>() > 0)
                {
                    //save modified
                    if (modified.Count<WsObject>() > 0)
                    {
                        await _restClient.PutItemsAsync(modified);
                    }
                }
            }
        }

        private async Task SaveDeletedObjects()
        {
            foreach (var stateManagerEntry in _setStatesManagers)
            {
                IEnumerable<WsObject> deleted = stateManagerEntry.Value.GetDeleteObjects();
                if (deleted.Count<WsObject>() > 0)
                {
                    //_restClient.Dele
                    //delete 
                    if (_restClient != null)
                    {
                        foreach (WsObject currentItem in deleted)
                        {
                            await _restClient.DeleteItemAsync(currentItem);
                            // _restClient.Dete
                        }
                        // _restClient.DeleteItems(deleted);
                    }
                    // t.GetRuntimeMethod("Add",);
                }
            }
        }

        public async Task SaveAllAsync()
        {
            await SaveAddedObjects();
            await SaveModifedObjects();
            await SaveDeletedObjects();
            
        }
        public void SaveAll()
        {
            SaveAllAsync().RunSynchronously();      
        }

        public IWsSet<TEntity> Set<TEntity>() where TEntity : WsObject
        {
            
            IWsSet<TEntity> retSet = null;
            string entityName = nameof(TEntity);
            if (!_setStatesManagers.ContainsKey(entityName))
            {
                _setStatesManagers[entityName] = new ObjectStateManager();
            }

            if (_availableSets.ContainsKey(entityName))
            {
                retSet = _availableSets[entityName] as IWsSet<TEntity>;
            }
            else
            {
                //replace this with a factory call 
                retSet = new WsSet<TEntity>(this, _setStatesManagers[entityName]);// as IWsSet<TEntity>;
                _availableSets[entityName] = retSet as WsSet<TEntity>;
            }
            return retSet;
        }

        public IRestClient RESTClient
        {
            get
            {
                return _restClient;
            }            
        }

    }
}
