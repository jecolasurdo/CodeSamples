using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public class Edge : IEdge
    {
        public int Id { get; private set; }
        public Node FromNode { get; private set; }
        public Node ToNode { get; private set; }
        public string EdgeName { get; private set; }

        public Edge(int id, string edgeName, Node fromNode, Node toNode) {
            Id = id;
            FromNode = fromNode;
            ToNode = toNode;
            EdgeName = edgeName;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var n = obj as IEdge;
            if ((System.Object)n == null)
            {
                return false;
            }

            return (Id == n.Id);
        }

        [DebuggerStepThrough]
        public bool Equals(IEdge e)
        {
            if ((object)e == null)
            {
                return false;
            }

            return (Id == e.Id);
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            return Id;
        }

        [DebuggerStepThrough]
        public static bool operator ==(Edge a, Edge b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Id == b.Id;
        }

        [DebuggerStepThrough]
        public static bool operator !=(Edge a, Edge b)
        {
            return !(a == b);
        }

    }
}
