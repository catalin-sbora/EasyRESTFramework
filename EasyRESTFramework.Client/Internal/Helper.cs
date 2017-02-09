using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client.Internal
{
    public class Helper
    {
        public static void ShallowCopy<TEntity>(TEntity source, TEntity destination)
        {
            var properties = source.GetType().GetRuntimeProperties().Where(property => property.CanWrite == true);
            foreach (PropertyInfo property in properties)
            {
                if (!typeof(IEnumerable<>).GetTypeInfo().IsAssignableFrom(property.GetType().GetTypeInfo()))
                {
                    var fieldValue = property.GetValue(source);
                    property.SetValue(destination, fieldValue);
                }
                else
                {
                    property.SetValue(destination, null);
                }
            }
        }
    }
}
