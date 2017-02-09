using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Filters;

namespace EasyRESTFramework.Client.Abstractions
{
    /*REST Client async */
    public interface IRestClientAsync
    {
        //we need to specify conditions
        Task<TEntity> GetItemAsync<TEntity>(int itemId, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject;
        Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(QueryFilter filter = null, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject;
        Task DeleteItemAsync<TEntity>(TEntity itemToDelete, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject;
        Task<TEntity> PostItemAsync<TEntity>(TEntity itemToPost, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject;
        Task<IEnumerable<TEntity>> PostItemsAsync<TEntity>(IEnumerable<TEntity> itemsToPost, Type collectionType, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject;
        Task PutItemAsync<TEntity>(TEntity itemToPut, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject;
        Task PutItemsAsync<TEntity>(IEnumerable<TEntity> itemsToPut, CancellationToken cancelToken = default(CancellationToken)) where TEntity : WsObject;


       /* TEntity GetItem<TEntity>(int itemId) where TEntity : WsObject;
        IEnumerable<TEntity> GetItems<TEntity>(QueryFilter filter = null) where TEntity : WsObject;
        void DeleteItem<TEntity>(TEntity itemToDelete) where TEntity : WsObject;
        TEntity PostItem<TEntity>(TEntity itemToPost) where TEntity : WsObject;
        IEnumerable<TEntity> PostItems<TEntity>(IEnumerable<TEntity> itemsToPost) where TEntity : WsObject;
        void PutItem<TEntity>(TEntity itemToPut) where TEntity : WsObject;
        void PutItems<TEntity>(IEnumerable<TEntity> itemsToPut) where TEntity : WsObject;  */
        
    }
}
