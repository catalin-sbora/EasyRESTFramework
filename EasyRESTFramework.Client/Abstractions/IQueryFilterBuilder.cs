using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Filters;
namespace EasyRESTFramework.Client.Abstractions
{
    public interface IQueryFilterBuilder
    {
        string CreateStringFilter(QueryFilter filter);
        Func<TEntity, bool> CreateExpressionFilter<TEntity>(QueryFilter filter);
    }
}
