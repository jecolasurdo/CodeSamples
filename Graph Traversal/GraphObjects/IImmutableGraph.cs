using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public interface IImmutableGraph
    {
        EdgeCollection Edges { get; }
        PathCollection Paths { get; }
        List<Node> Nodes { get; }
        void FindAllPaths();
    }
}
