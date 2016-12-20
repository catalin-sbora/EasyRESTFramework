using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client.Filters
{
    public class QueryFilter
    {
        private List<QueryFilterCriteria> _criterias = new List<QueryFilterCriteria>();

        public void AddCriteria(QueryFilterCriteria criteria)
        {
            _criterias.Add(criteria);
        }

        public IEnumerable<QueryFilterCriteria> Criterias
        {
            get { return _criterias; }
        }
    }
}
