using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Abstractions;

namespace EasyRESTFramework.Client
{
    public class JSONSerializer : IEntityTextSerializer
    {
        public TEntity DeserializeTextAsEntity<TEntity>(string textToDeserialize) where TEntity : WsObject
        {
            throw new NotImplementedException();
        }

        public string SerializeEntity<TEntity>(TEntity entityToSerialize) where TEntity : WsObject
        {
            throw new NotImplementedException();
        }
    }
}
