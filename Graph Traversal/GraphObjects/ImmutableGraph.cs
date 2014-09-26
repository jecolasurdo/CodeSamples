using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public class ImmutableGraph : IImmutableGraph
    {
        public EdgeCollection Edges { get; private set; }
        public PathCollection Paths { get; private set; }
        public List<Node> Nodes { get; private set; }

        public ImmutableGraph(EdgeCollection edges) {
            Edges = edges;
            Nodes = Edges.NodeList;
        }

        public void FindAllPaths() {
            Paths = Edges.FindAllPaths();
        }
    }
}
