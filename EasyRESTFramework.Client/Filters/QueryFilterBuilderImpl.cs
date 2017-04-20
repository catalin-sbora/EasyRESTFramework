using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Abstractions;
using System.Net.Http;
using System.Net;
using System.Reflection;

namespace EasyRESTFramework.Client.Filters
{
    public class QueryFilterBuilderImpl : IQueryFilterBuilder
    {
       
        public QueryFilterBuilderImpl()
        {
            
        }
        private bool ConditionMatchesWithParam(string propertyValue, string valueToCheck, CriteriaCheckCondition condition)
        {
            bool bRet = false;
            switch (condition)
            {
                case CriteriaCheckCondition.Equals:
                    bRet = propertyValue.Equals(valueToCheck);            
                    break;

                case CriteriaCheckCondition.GreaterThan:
                    
                    break;

                case CriteriaCheckCondition.LessThan:

                    break;
                case CriteriaCheckCondition.GreaterOrEqual:

                    break;
                case CriteriaCheckCondition.LessOrEqual:

                    break;
                case CriteriaCheckCondition.NotEqual:
                    bRet = (propertyValue.Equals(valueToCheck) == false);
                    break;
                case CriteriaCheckCondition.StartsWith:
                    bRet = propertyValue.StartsWith(valueToCheck);
                    break;
                case CriteriaCheckCondition.EndsWith:
                    bRet = propertyValue.EndsWith(valueToCheck);
                    break;
                case CriteriaCheckCondition.Contains:
                    bRet = propertyValue.Contains(valueToCheck);
                    break;
                default:
                    break;
            }
            return bRet;
        }
        private string CriteriaCheckCondToString(CriteriaCheckCondition condition)
        {
            string strRet = "";
            switch (condition)      
            {
                case CriteriaCheckCondition.Equals:
                    strRet = "__eq__";
                    break;

                case CriteriaCheckCondition.GreaterOrEqual:
                    strRet = "__ge__";
                    break;

                case CriteriaCheckCondition.GreaterThan:
                    strRet = "__gt__";
                    break;

                case CriteriaCheckCondition.LessOrEqual:
                    strRet = "__le__";
                    break;

                case CriteriaCheckCondition.LessThan:
                    strRet = "__lt__";
                    break;

                case CriteriaCheckCondition.NotEqual:
                    strRet = "__ne__";
                    break;
                case CriteriaCheckCondition.Contains:
                    strRet = "__contains__";
                    break;

            } 

            return strRet;
        }

        private string CriteriaTypeToString(CriteriaType type)
        {
            string retString = "";
            switch (type)
            {
                case CriteriaType.Optional:
                    retString = "__or__";
                    break;
                case CriteriaType.Required:
                    retString = "__and__";
                    break;
                default:
                    retString = "__or__";
                    break;
            }
            return retString;
        }

        public string CreateStringFilter(QueryFilter filter)
        {
            string retFilter = "?filter=";
            string currentFilter = "";
            bool firstIteration = true;
            IEnumerable<QueryFilterCriteria> requiredCriterias = filter.Criterias.Where(criteria => criteria.Type == CriteriaType.Required);
            IEnumerable<QueryFilterCriteria> optionalCriterias = filter.Criterias.Where(criteria => criteria.Type == CriteriaType.Optional);

            foreach (QueryFilterCriteria requiredCriteria in requiredCriterias)
            {
                if (firstIteration != true)
                {
                    currentFilter += CriteriaTypeToString(requiredCriteria.Type);
                }

                currentFilter += requiredCriteria.Name + CriteriaCheckCondToString(requiredCriteria.CheckCondition) + requiredCriteria.ValueToCheck;
                firstIteration = false;
            }

            retFilter +=  WebUtility.UrlEncode(currentFilter);
            return retFilter;
        }

        public Func<TEntity, bool> CreateExpressionFilter<TEntity>(QueryFilter filter)
        {
            Func<TEntity, bool> retFunc = (TEntity entity) => 
            {
                bool retVal = false;
                IEnumerable<PropertyInfo> properties = typeof(TEntity).GetRuntimeProperties();
                Dictionary<string, PropertyInfo> propertiesDic = new Dictionary<string, PropertyInfo>();
                foreach (PropertyInfo prop in properties)
                {
                    propertiesDic.Add(prop.Name, prop);
                }
                
                foreach (QueryFilterCriteria criteria in filter.Criterias.Where(c => c.Type == CriteriaType.Optional))
                {
                    //check to see if we can find the criteria in the properties
                    if (propertiesDic.ContainsKey(criteria.Name))
                    {
                        PropertyInfo currentProperty = propertiesDic[criteria.Name];
                        if (currentProperty.CanRead)
                        {
                            object propertyValue = currentProperty.GetValue(entity);
                            string propertyStringValue = propertyValue.ToString();
                            retVal = ConditionMatchesWithParam(propertyStringValue, criteria.ValueToCheck, criteria.CheckCondition);
                        }
                        if (retVal)
                            break;
                    }
                }

                if (retVal == false)
                {
                    foreach (QueryFilterCriteria criteria in filter.Criterias.Where(c => c.Type == CriteriaType.Required))
                    {
                        PropertyInfo currentProperty = propertiesDic[criteria.Name];
                        object propertyValue = currentProperty.GetValue(entity);
                        string propertyStringValue = propertyValue.ToString();
                        retVal = ConditionMatchesWithParam(propertyStringValue, criteria.ValueToCheck, criteria.CheckCondition);
                        if (retVal == false)
                            break;
                    }
                }

                return retVal;
            };

            return retFunc;
        }
    }
}
