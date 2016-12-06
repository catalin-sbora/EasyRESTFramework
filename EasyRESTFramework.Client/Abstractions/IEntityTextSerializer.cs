using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client.Abstractions
{
    public interface IEntityTextSerializer
    {
        String SerializeEntity<TEntity>(TEntity entityToSerialize) where TEntity: WsObject;
        TEntity DeserializeTextAsEntity<TEntity>(String textToDeserialize) where TEntity : WsObject;
    }
}
