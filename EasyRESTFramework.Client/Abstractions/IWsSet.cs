using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EasyRESTFramework.Client.Abstractions
{
    public interface IWsSet<TEntity> where TEntity:WsObject
    {
        bool Add(TEntity toAdd);

        bool Remove(TEntity toRemove);

        bool Update(TEntity toUpdate);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> FindEntities(params object[] keys);

        //Dictionary<string, string> GetTextSerializedData(bool onlyChangedEntities = true); 
    }
}
