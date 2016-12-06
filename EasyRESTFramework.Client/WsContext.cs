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
        private readonly IEntityTextSerializer _serializer;
        private readonly IRestClient _restClient;

        public WsContext(IRestClient restClient) : this(restClient, new JSONSerializer())
        {
            //use Json serializer as default
        }

        public WsContext(IRestClient restClient, IEntityTextSerializer serializer)
        {
            _restClient = restClient;
            _serializer = serializer;           
        }

        public void SaveAll()
        {
            foreach (var stateManagerEntry in _setStatesManagers)
            {
                IEnumerable<WsObject> added = stateManagerEntry.Value.GetAddedObjects();
                if (added.Count<WsObject>() > 0)
                {
                    //save added
                    if (_restClient != null)
                    {                        
                         _restClient.PostItems(added);               
                    }
                }

                IEnumerable<WsObject> modified = stateManagerEntry.Value.GetModifiedObjects();
                if (modified.Count<WsObject>() > 0)
                {
                    //save modified
                    if (modified.Count<WsObject>() > 0)
                    {
                        _restClient.PutItems(modified);
                    }
                }

                IEnumerable<WsObject> deleted = stateManagerEntry.Value.GetDeleteObjects();
                if (deleted.Count<WsObject>() > 0)
                {
                    //_restClient.Dele
                    //delete 
                    if (_restClient != null)
                    {
                        _restClient.DeleteItems(deleted);
                    }
                   // t.GetRuntimeMethod("Add",);
                }
            }         
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

        

    }
}
