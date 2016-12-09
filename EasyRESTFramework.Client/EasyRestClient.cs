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

namespace EasyRESTFramework.Client
{
    public class EasyRESTClient : IRestClient
    {
        private readonly string _baseURL = "";
        private readonly HttpClient _client;
        private List<MediaTypeFormatter> _mediaFormatters = new List<MediaTypeFormatter>(); 
        public EasyRESTClient(string baseURL)
        {            
            _baseURL = baseURL;
            _client = new HttpClient();            
            _client.BaseAddress = new System.Uri(_baseURL);
            _mediaFormatters.Add(new JsonMediaTypeFormatter());
            _mediaFormatters.Add(new XmlMediaTypeFormatter());         
            
        }
        private string getItemUri<TEntity>(TEntity itemType) where TEntity: WsObject
        {
            string retVal = "";
            var type = typeof(TEntity);
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

        public async Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            IEnumerable<TEntity> retList = null;

            var requestUri = getCollectionUri<TEntity>();
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

        public async Task<TEntity> PostItemAsync<TEntity>(TEntity itemToPost, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            var requestUri = "";
            TEntity newEntityData = null;
            if (itemToPost.HasValidId())
            {
                requestUri = getItemUri(itemToPost);
            }
            else
            {
                requestUri = getCollectionUri<TEntity>();
            }
            //todo: post data depending on the selected media formatter
            var response = await _client.PostAsJsonAsync(requestUri, itemToPost, cancelToken);
            response.EnsureIsSuccessStatusCode();
            newEntityData = await response.Content.ReadAsAsync<TEntity>(_mediaFormatters, cancelToken);

            return newEntityData;
        }

        public async Task<IEnumerable<TEntity>> PostItemsAsync<TEntity>(IEnumerable<TEntity> itemsToPost, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject
        {
            IEnumerable<TEntity> retList = null;

            var requestUri = getCollectionUri<TEntity>();
            var response = await _client.PostAsJsonAsync<IEnumerable<TEntity>>(requestUri, itemsToPost, cancelToken);
            response.EnsureIsSuccessStatusCode();
            retList = await response.Content.ReadAsAsync<List<TEntity>>(_mediaFormatters, cancelToken);

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


        public TEntity GetItem<TEntity>(int itemId) where TEntity : WsObject
        {
            var retVal = GetItemAsync<TEntity>(itemId).Result;
            return retVal;
        }

        public IEnumerable<TEntity> GetItems<TEntity>() where TEntity : WsObject
        {
            var retVal = GetItemsAsync<TEntity>().Result;

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

        
    }

            
}
