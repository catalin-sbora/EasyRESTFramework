using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace EasyRESTFramework.Client.Internal
{
    internal class TypeDependencyGraphBuilder
    {
        private List<Type> _types = null;
        private HashSet<string> _availableTypeNames = new HashSet<string>();
        private DependencyGraph _lastGraph = null;


        public DependencyGraph LastGraph
        {
            get { return _lastGraph; }
            private set { }
        }

        public TypeDependencyGraphBuilder(List<Type> types)
        {
            _types = types;
            if (_types != null)
            {
                foreach (Type t in _types)
                {
                    _availableTypeNames.Add(t.Name);
                }
            }
        }

        private List<String> GetDependenciesForType(Type t)
        {
            List<String> retList = new List<String>();
            return retList;
        }
        public DependencyGraph BuildDependencyGraph()
        {
            DependencyGraph depGraph = new DependencyGraph();            
            //for each type in the list 
            foreach (Type currentType in _types)
            {
                var typeName = currentType.Name;
                var properties = currentType.GetRuntimeProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetMethod != null && property.GetMethod.IsPublic)
                    {
                        //the property is public
                        var propertyType = property.GetType();
                        if (_availableTypeNames.Contains(propertyType.Name) && !propertyType.Name.Equals(typeName))
                        {
                            //add dependency
                            depGraph.AddDependency(typeName, propertyType.Name);
                        }
                    }
                }
            }
            return depGraph;
        }
    }
}
