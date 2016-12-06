
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Internal;
using EasyRESTFramework.Client.Abstractions;

namespace EasyRESTFramework.Client
{
    public class WsSet<TEntity> : IWsSet<TEntity> where TEntity: WsObject
    {
        private readonly IWsContext _wsContext;
        private readonly HashSet<TEntity> _entities = new HashSet<TEntity>();
        private readonly ObjectStateManager _stateManager = new ObjectStateManager();
        //private readonly HashSet<EntityContainer> _collection = new HashSet<EntityContainer>();
        
        internal WsSet(IWsContext wsContext, ObjectStateManager stateManager)
        {
            this._wsContext = wsContext;
        }
        public WsSet(IWsContext wsContext): this(wsContext, null)
        {
           
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
                addResult =_entities.Add(toAdd);
            }

            return addResult;
        }

        public bool Remove(TEntity toRemove)
        {
            throw new NotImplementedException();
        }

        public bool Update(TEntity toUpdate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindEntities(params object[] keys)
        {
            throw new NotImplementedException();
        }        

        
                

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        
        public Type GetStoredType()
        {
            throw new NotImplementedException();
        }

        

        
    }
}
