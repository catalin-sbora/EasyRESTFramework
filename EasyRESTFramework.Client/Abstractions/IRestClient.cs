using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client.Abstractions
{
    public interface IRestClient
    {
        //we need to specify conditions
        IEnumerable<TEntity> GetItems<TEntity>();
        Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>();

        TEntity PostItem<TEntity>(TEntity itemToPost);
        Task<TEntity> PostItemAsync<TEntity>(TEntity itemToPost);

        IEnumerable<TEntity> PostItems<TEntity>(IEnumerable<TEntity> itemsToPost);
        Task<IEnumerable<TEntity>> PostItemsAsync<TEntity>(IEnumerable<TEntity> itemsToPost);

        void PutItem<TEntity>(TEntity itemToPut);
        Task PutItemAsync<TEntity>(TEntity itemToPut);

        void PutItems<TEntity>(IEnumerable<TEntity> itemsToPut);
        Task PutItemsAsync<TEntity>(IEnumerable<TEntity> itemsToPut);

        void DeleteItem<TEntity>(TEntity itemToDelete);
        Task DeleteItemAsync<TEntity>(TEntity itemToDelete);

        void DeleteItems<TEntity>(IEnumerable<TEntity> itemsToDelete);
        Task DeleteItemsAsync<TEntity>(IEnumerable<TEntity> itemsToDeleted);
    }
}
