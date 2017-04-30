using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using EasyRESTFramework.Client.Abstractions;
using EasyRESTFramework.Client.Extensions;
using EasyRESTFramework.Client.Filters;
using Newtonsoft.Json;
namespace EasyRESTFramework.Client
{
    public class EasyRESTClient : IRestClientAsync
    {
        private readonly string _baseURL = "";
        private readonly HttpClient _client;        
        private readonly IQueryFilterBuilder _queryFilterBuilder;
        public EasyRESTClient(string baseURL, IQueryFilterBuilder filterBuilder = null)
        {
            string error;
            _baseURL = baseURL;
            _client = new HttpClient();
            if (filterBuilder != null)
            {
                _queryFilterBuilder = filterBuilder;
                
            }
            else
            {
                _queryFilterBuilder = new QueryFilterBuilderImpl();
            }
            _client.BaseAddress = new System.Uri(_baseURL);
            
        }
        private string getItemUri<TEntity>(TEntity itemType) where TEntity: WsObject
        {
            string retVal = "";
            var type = itemType.GetType();
            retVal = type.Name.ToLower();
            retVal += "s/" + itemType.Id;    
            return retVal;
        }

        private string getItemUri<TEntity>(int itemId) where TEntity : WsObject
        {
            string retVal = "";
            var type = typeof(TEntity);
            retVal = type.Name.ToLower();
            retVal += "s/" + itemId;
            return retVal;
        }

        private string getCollectionUri<TEntity>(TEntity firstItemInCollection) where TEntity : WsObject
        {
            string retVal = "";
            var type = firstItemInCollection.GetType();
            retVal = type.Name.ToLower();
            retVal += "s";
            return retVal;
        }

        private string getCollectionUri<TEntity>() where TEntity : WsObject
        {
            string retVal = "";
            var type = typeof(TEntity);
            retVal = type.Name.ToLower();
            retVal += "s";
            return retVal;
        }

        public async Task<TEntity> GetItemAsync<TEntity>(int itemId, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            var requestUri = getItemUri<TEntity>(itemId);
            var httpRespose = await _client.GetAsync(requestUri);
            httpRespose.EnsureIsSuccessStatusCode();
            var responseString = await httpRespose.Content.ReadAsStringAsync();
            var retItem = JsonConvert.DeserializeObject<TEntity>(responseString);
            return retItem;
        }

        public async Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(QueryFilter filter = null, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            IEnumerable<TEntity> retList = null;

            var requestUri = getCollectionUri<TEntity>();
            if (filter != null)
            {
                //consider injecting the filter builder and make the builder receive the filter as parameter for its methods

                string stringFilter = _queryFilterBuilder.CreateStringFilter(filter);
                requestUri += stringFilter;
            }
            
            var httpResponse = await _client.GetAsync(requestUri);
            httpResponse.EnsureIsSuccessStatusCode();
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            retList = JsonConvert.DeserializeObject<List<TEntity>>(responseString);

            return retList;
        }

        public async Task DeleteItemAsync<TEntity>(TEntity itemToDelete, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            var itemUri = getItemUri(itemToDelete);
            var response = await _client.DeleteAsync(itemUri, cancelToken);
            response.EnsureIsSuccessStatusCode();
        }

        public async Task<TEntity> PostItemAsync<TEntity>(TEntity itemToPost,  CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            var requestUri = "";
            TEntity newEntityData = null;
            if (itemToPost.HasValidId())
            {
                requestUri = getItemUri(itemToPost);
            }
            else
            {                
                requestUri = getCollectionUri<TEntity>(itemToPost);
            }
            var serializedData = JsonConvert.SerializeObject(itemToPost);
            var responseContent = new StringContent(serializedData, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(requestUri, responseContent, cancelToken);
            response.EnsureIsSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            newEntityData = JsonConvert.DeserializeObject<TEntity>(responseString);

            return newEntityData;
        }

        public async Task<IEnumerable<TEntity>> PostItemsAsync<TEntity>(IEnumerable<TEntity> itemsToPost, Type collectionType, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            IEnumerable<TEntity> retList = null;
            if (itemsToPost.Count() > 0)
            {
                var typeToRead = collectionType;
                var firstItem = itemsToPost.First();
                var requestUri = getCollectionUri(firstItem);
                var serializedList = JsonConvert.SerializeObject(itemsToPost);
                var stringResult = new StringContent(serializedList, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(requestUri, stringResult, cancelToken);
                if (typeToRead == null)
                { 
                    typeToRead = itemsToPost.GetType();
                }
                response.EnsureIsSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                retList = JsonConvert.DeserializeObject<IEnumerable<TEntity>>(responseString);
              
            }
            return retList;
        }

        public async Task PutItemAsync<TEntity>(TEntity itemToPut, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            var requestURI = getItemUri(itemToPut);
            var jsonString = JsonConvert.SerializeObject(itemToPut);
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(requestURI, jsonContent, cancelToken);
            response.EnsureIsSuccessStatusCode();           
        }

        public async Task PutItemsAsync<TEntity>(IEnumerable<TEntity> itemsToPut, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            var requestURI = getCollectionUri<TEntity>();
            var jsonString = JsonConvert.SerializeObject(itemsToPut);
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(requestURI, jsonContent, cancelToken);
            response.EnsureIsSuccessStatusCode();
        }

        /*
        public TEntity GetItem<TEntity>(int itemId) where TEntity : WsObject
        {
            var retVal = GetItemAsync<TEntity>(itemId).Result;
            return retVal;
        }

        public IEnumerable<TEntity> GetItems<TEntity>(QueryFilter filter) where TEntity : WsObject
        {
            var retVal = GetItemsAsync<TEntity>(filter).Result;

            return retVal;
        }

        public void DeleteItem<TEntity>(TEntity itemToDelete) where TEntity : WsObject
        {
            this.DeleteItemAsync(itemToDelete).RunSynchronously();
        }

        public TEntity PostItem<TEntity>(TEntity itemToPost) where TEntity : WsObject
        {
            var retVal = PostItemAsync(itemToPost).Result;
            return retVal;
        }

        public IEnumerable<TEntity> PostItems<TEntity>(IEnumerable<TEntity> itemsToPost) where TEntity : WsObject
        {
            var retVal = PostItemsAsync(itemsToPost).Result;

            return retVal;
        }

        public void PutItem<TEntity>(TEntity itemToPut) where TEntity : WsObject
        {
            PutItemAsync(itemToPut).RunSynchronously();
        }

        public void PutItems<TEntity>(IEnumerable<TEntity> itemsToPut) where TEntity : WsObject
        {
            PutItemsAsync(itemsToPut).RunSynchronously();
        }
        */
    }

            
}
