using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using EasyRESTFramework.Client.Abstractions;
using EasyRESTFramework.Client.Extensions;
using EasyRESTFramework.Client.Filters;

namespace EasyRESTFramework.Client
{
    public class EasyRESTClient : IRestClientAsync
    {
        private readonly string _baseURL = "";
        private readonly HttpClient _client;
        private List<MediaTypeFormatter> _mediaFormatters = new List<MediaTypeFormatter>();
        private readonly IQueryFilterBuilder _queryFilterBuilder;
        public EasyRESTClient(string baseURL, IQueryFilterBuilder filterBuilder = null)
        {            
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
            _mediaFormatters.Add(new JsonMediaTypeFormatter());
            _mediaFormatters.Add(new XmlMediaTypeFormatter());         
            
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
            var retItem = await httpRespose.Content.ReadAsAsync<TEntity>(_mediaFormatters);
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
            retList = await httpResponse.Content.ReadAsAsync<List<TEntity>>(_mediaFormatters);

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
            //todo: post data depending on the selected media formatter
            var response = await _client.PostAsJsonAsync(requestUri, itemToPost, cancelToken);
            response.EnsureIsSuccessStatusCode();
            newEntityData = await response.Content.ReadAsAsync<TEntity>(_mediaFormatters, cancelToken);

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
                var response = await _client.PostAsJsonAsync(requestUri, itemsToPost, cancelToken);
                if (typeToRead == null)
                { 
                    typeToRead = itemsToPost.GetType();
                }
                response.EnsureIsSuccessStatusCode();
                retList = await response.Content.ReadAsAsync(typeToRead, _mediaFormatters, cancelToken) as IEnumerable<TEntity>;
                //retList = result;
            }
            return retList;
        }

        public async Task PutItemAsync<TEntity>(TEntity itemToPut, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            var requestURI = getItemUri(itemToPut);
            var response = await _client.PutAsJsonAsync<TEntity>(requestURI, itemToPut, cancelToken);
            response.EnsureIsSuccessStatusCode();           
        }

        public async Task PutItemsAsync<TEntity>(IEnumerable<TEntity> itemsToPut, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            var requestURI = getCollectionUri<TEntity>();
            var response = await _client.PutAsJsonAsync<IEnumerable<TEntity>>(requestURI, itemsToPut, cancelToken);
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
