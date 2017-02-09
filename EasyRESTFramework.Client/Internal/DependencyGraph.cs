using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client.Internal
{
    internal class DependencyGraph
    {
        private Dictionary<String, GraphNode> _nodes = new Dictionary<String, GraphNode>();

        public void AddDependency(String baseEntityName, String dependencyName)
        {
            if (!_nodes.ContainsKey(baseEntityName))
            {
                _nodes.Add(baseEntityName, new GraphNode(baseEntityName));
            }
            if (!_nodes.ContainsKey(dependencyName))
            {
                _nodes.Add(dependencyName, new GraphNode(dependencyName));
            }

            GraphNode baseNode = _nodes[baseEntityName];
            GraphNode dependency = _nodes[dependencyName];
            baseNode.AddDependency(dependency);
        }

        private GraphNode GetFirstNodeWithoutResponsability(HashSet<String> visitedNodes)
        {
            GraphNode retNode = null;
            foreach (GraphNode _nodeX in _nodes.Values)
            {
                if (!visitedNodes.Contains(_nodeX.NodeName))
                {
                    if (_nodeX.DependingNodesCount == 0)
                    {
                        retNode = _nodeX;
                        break;
                    }
                }
            }
            return retNode;
        }

        public IEnumerable<String> GetExecutionList()
        {
            List<String> retList = new List<string>();
            Stack<GraphNode> nodesStack = new Stack<GraphNode>();
            HashSet<String> visitedNodes = new HashSet<String>();
            GraphNode currentNode = null;

            while (visitedNodes.Count < _nodes.Count)
            {
                currentNode = GetFirstNodeWithoutResponsability(visitedNodes);
                if (currentNode != null)
                {
                    retList.Add(currentNode.NodeName);
                    visitedNodes.Add(currentNode.NodeName);
                    //nodesStack.Push(firstNode);
                    while (currentNode != null)
                    {
                        var nodeIterator = currentNode.GetFirstDependency();
                        while (nodeIterator != null)
                        {
                            //keep the entry that was added last
                            if (retList.Contains(nodeIterator.NodeName))
                                    retList.Remove(nodeIterator.NodeName);

                            retList.Add(nodeIterator.NodeName);
                            visitedNodes.Add(nodeIterator.NodeName);
                            //if (!visitedNodes.Contains(nodeIterator.NodeName))
                            nodesStack.Push(nodeIterator);
                            nodeIterator = currentNode.GetNextDependency();
                        }
                        if (nodesStack.Count > 0)
                            currentNode = nodesStack.Pop();
                        else
                            currentNode = null;
                    }
                }
                else
                {
                    //should break here
                }
            }
            retList.Reverse();
            return retList;
        }
    }
}
