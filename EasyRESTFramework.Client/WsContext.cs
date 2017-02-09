using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Abstractions;
using EasyRESTFramework.Client.Internal;
using System.Reflection;
using EasyRESTFramework.Client.Filters;

namespace EasyRESTFramework.Client
{
    public class WsContext: IWsContext
    {
        //private Dictionary<string, IStateAccessor> dataSets = new Dictionary<string, IStateAccessor>();
        private readonly Dictionary<String, ObjectStateManager> _setStatesManagers = new Dictionary<string, ObjectStateManager>();
        private readonly Dictionary<String, object> _availableSets = new Dictionary<string, object>();
        private readonly List<Type> _setsTypeList = new List<Type>();
        private readonly HashSet<String> _registeredSets = new HashSet<String>();
        private readonly IRestClientAsync _restClient;
        private readonly IQueryFilterBuilder _filterBuilder;
        private TypeDependencyGraphBuilder _dependencyBuilder = null;
        private bool _setsListChanged = true;
        

        public WsContext(IRestClientAsync restClient, IQueryFilterBuilder filterBuilder = null)
        {
            _restClient = restClient;
            if (filterBuilder == null)
                _filterBuilder = new QueryFilterBuilderImpl();
            else
                _filterBuilder = filterBuilder;
        }

        private async Task SaveAddedObjects()
        {
            DependencyGraph depGraph = null;
            ICollection<String> executionList = null;
            //check dependency for each type and create a graph of dependencies
            //after the graph is created go through the nodes in order
            if (_setsListChanged)
            {
                _dependencyBuilder = new TypeDependencyGraphBuilder(_setsTypeList);
                depGraph = _dependencyBuilder.BuildDependencyGraph();
                _setsListChanged = false;
            }
            else
            {
                depGraph = _dependencyBuilder.LastGraph;
            }

            if (depGraph != null)
            {
                executionList = depGraph.GetExecutionList() as ICollection<String>;
            }
            else
            {
                throw new Exception("Failed to get access to the dependency graph.");
            }

            for (int idx = 0; idx < executionList.Count; idx++)
            {
                if (_setStatesManagers.ContainsKey(executionList.ElementAt(idx)))
                {
                    var stateManagerEntry = _setStatesManagers[executionList.ElementAt(idx)];
                    IEnumerable<WsObject> added = stateManagerEntry.GetAddedObjects();
                    if (added.Count<WsObject>() > 0)
                    {
                        //save added
                        if (_restClient != null)
                        {
                            IEnumerable<WsObject> postedItems = await _restClient.PostItemsAsync(added, stateManagerEntry.BaseItemType);
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
                else
                {

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
            try
            {
                await SaveAddedObjects();
                foreach (var stateManagerEntry in _setStatesManagers)
                {
                    stateManagerEntry.Value.MarkAddedObjectsAsSaved();
                }

                await SaveModifedObjects();
                foreach (var stateManagerEntry in _setStatesManagers)
                {
                    stateManagerEntry.Value.MarkModifiedObjectsAsSaved();
                }

                await SaveDeletedObjects();
                foreach (var stateManagerEntry in _setStatesManagers)
                {
                    stateManagerEntry.Value.MarkDeletedObjectsAsSaved();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveAll()
        {
            SaveAllAsync().RunSynchronously();      
        }

        public IWsSet<TEntity> Set<TEntity>() where TEntity : WsObject
        {            
            IWsSet<TEntity> retSet = null;
            string entityName = typeof(TEntity).Name;
            if (!_registeredSets.Contains(entityName))
            {
                _registeredSets.Add(entityName);
            }
            if (!_setStatesManagers.ContainsKey(entityName))
            {
                _setStatesManagers[entityName] = new ObjectStateManager(typeof(IEnumerable<TEntity>));
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
                _setsTypeList.Add(typeof(TEntity));
                _setsListChanged = true;
            }

            return retSet;
        }

        public IRestClientAsync RESTClient
        {
            get
            {
                return _restClient;
            }            
        }

        public IQueryFilterBuilder FilterBuilder
        {
            get
            {
                return _filterBuilder;
            }
        }
    }
}
