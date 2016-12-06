using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client.Abstractions
{
    public interface IWsContext
    {
        IWsSet<TEntity> Set<TEntity>() where TEntity: WsObject;
        void SaveAll();
    }
}
