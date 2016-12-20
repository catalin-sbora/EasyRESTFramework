using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client.Filters
{
    public enum CriteriaCheckCondition
    {
        Equals = 0,
        GreaterThan = 1,
        LessThan = 2,
        GreaterOrEqual = 3,
        LessOrEqual = 4,
        NotEqual = 5,
        Contains = 6,
        StartsWith = 7,
        EndsWith = 8
    };

    public enum CriteriaType
    {
        Optional = 0,
        Required = 1
    };

        
    public class QueryFilterCriteria
    {
        private CriteriaCheckCondition _criteriaCheckCondition = CriteriaCheckCondition.Equals;
        private CriteriaType _criteriaType = CriteriaType.Optional;
        private string _criteriaName = "";
        private string _checkValue = "";
        public QueryFilterCriteria(string criteriaName, CriteriaCheckCondition checkCondition, string checkValue, CriteriaType type)
        {
            _criteriaCheckCondition = checkCondition;
            _criteriaType = type;
            _criteriaName = criteriaName;
            _checkValue = checkValue;              
        }

        public CriteriaCheckCondition CheckCondition
        {
            get { return _criteriaCheckCondition; }
        }

        public CriteriaType Type
        {
            get { return _criteriaType; }
        }

        public string Name
        {
            get { return _criteriaName; }
        }

        public string ValueToCheck
        {
            get { return _checkValue; }
        }
       
    }
}
