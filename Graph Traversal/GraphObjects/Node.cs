using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public class Node : INode
    {

        public decimal Id { get; set; }
        public string Name { get; set; }

        [DebuggerStepThrough]
        public Node(decimal id, string name) {
            Id = id;
            Name = name;
        }

        [DebuggerStepThrough]
        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var n = obj as INode;
            if ((System.Object)n == null)
            {
                return false;
            }

            return (Id == n.Id);
        }

        [DebuggerStepThrough]
        public bool Equals(INode n)
        {
            // If parameter is null return false:
            if ((object)n == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Id == n.Id);
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            return (int)Math.Ceiling(Id);
        }

        [DebuggerStepThrough]
        public static bool operator ==(Node a, Node b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Id == b.Id;
        }

        [DebuggerStepThrough]
        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }

    }
}
