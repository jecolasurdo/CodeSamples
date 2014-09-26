using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public abstract class EdgeCollectionBase : List<IEdge>
    {
        protected EdgeCollectionBase() {
            NodeList = new List<Node>();
        }

        public List<Node> NodeList { get; private set; } 

        public void Add(Edge edge) {
            var misMatchedNode = NodeList.Find(existingNode => 
                                        (existingNode.Id == edge.FromNode.Id && existingNode.Name != edge.FromNode.Name) ||
                                        (existingNode.Id == edge.ToNode.Id && existingNode.Name != edge.ToNode.Name));
            if (misMatchedNode != null)
            {
                throw new Exception("Attempted to add a node to an edge collection where the node ID already exists in the collection, but node names differ.");
            }
                
            if (!NodeList.Contains(edge.ToNode))
            {
                NodeList.Add(edge.ToNode);
            }

            if (!NodeList.Contains(edge.FromNode))
            {
                NodeList.Add(edge.FromNode);
            }
            base.Add(edge);
        }

        public void Add(string edgeName, Node fromNode, Node toNode)
        {
            var maxEdgeId = 0;
            if (Count > 0)
            {
                maxEdgeId = this.Max(x => x.Id);
            }
            var newId = ++maxEdgeId;
            var newEdge = new Edge(newId, edgeName, fromNode, toNode);
            Add(newEdge);
        }

        public new void Insert(int index, IEdge item) {
            throw new NotImplementedException();
        }

        public new void InsertRange(int index, IEnumerable<IEdge> edgeCollection) {
            throw new NotImplementedException();
        }
    }
}
