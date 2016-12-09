using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client
{
    public class WsObject
    {
        public string Id
        {
            get;
            set;
        }

        public WsObject()
        {
            Id = "-1";
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(this.GetType()))
            {
                WsObject compare = (WsObject)obj;
                return Id.Equals(compare.Id);
            }
            return false;
        }

        public bool HasValidId()
        {
            return (Id.Equals("-1") == false);
        }
    }
}
