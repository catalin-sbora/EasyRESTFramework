using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client.Internal
{
    internal class GraphNode
    {

        private List<GraphNode> _dependencies = new List<GraphNode>();
        private int _currentIndex = 0;
        private String _nodeName = "";
        private int _dependingNodesCount = 0;

        public GraphNode(String nodeName)
        {
            _nodeName = nodeName;
        }

        public String NodeName
        {
            get { return _nodeName; }
            private set { _nodeName = value; }
        }

        public int DependenciesCount
        {
            get { return _dependencies.Count; }
            private set { }
        }
        public int DependingNodesCount
        {
            get { return _dependingNodesCount; }
            private set { _dependingNodesCount = value; }
        }

        public bool HasDependencies
        {
            get
            {
                return (_dependencies.Count > 0);
            }
            private set { }
        }

        public GraphNode GetFirstDependency()
        {
            _currentIndex = 0;
            return GetNextDependency();
        }

        public GraphNode GetNextDependency()
        {
            GraphNode retNode = null;
            if (_currentIndex < _dependencies.Count)
            {
                retNode = _dependencies.ElementAt(_currentIndex);
                _currentIndex++;
            }
            
            return retNode;
        }

        public void AddDependency(GraphNode nodeToAdd)
        {
            nodeToAdd.DependingNodesCount++;
            _dependencies.Add(nodeToAdd);
        }


    }
}
