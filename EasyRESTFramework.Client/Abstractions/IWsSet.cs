using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Filters;

namespace EasyRESTFramework.Client.Abstractions
{
    public interface IWsSet<TEntity> where TEntity:WsObject
    {
        bool Add(TEntity toAdd);

        bool Remove(TEntity toRemove);

        bool Update(TEntity toUpdate);

        /*IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetFilteredData(QueryFilter filter);*/

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetFilteredDataAsync(QueryFilter filter);

        //Dictionary<string, string> GetTextSerializedData(bool onlyChangedEntities = true); 
    }
}
