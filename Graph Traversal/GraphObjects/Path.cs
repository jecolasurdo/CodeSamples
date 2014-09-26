using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public class Path : EdgeCollectionBase, IPath
    {
        public INode FinalNode {
            get { return this.Last().ToNode; }
        }

        public INode FirstNode {
            get { return this.First().FromNode; }
        }

        public string EdgeListString {
            get {
                var edgeListString = "";
                foreach (var edge in this)
                {
                    edgeListString += edge.EdgeName + ",";
                }
                edgeListString = edgeListString.Remove(edgeListString.Length - 1, 1);
                return edgeListString;
            }
        }

        public string NodeListString {
            get {
                var nodeListString = "";
                foreach (var edge in this)
                {
                    nodeListString = edge.FromNode.Name + ",";
                }
                nodeListString = nodeListString.Remove(nodeListString.Length - 1, 1);
                return nodeListString;
            }
        }

        public string PathDescription {
            get {
                var pathDescription = "";
                foreach (var edge in this)
                {
                    pathDescription += edge.FromNode.Name + "\t" + edge.EdgeName;
                }
                pathDescription += this.Last().ToNode.Name;
                return pathDescription;
            }
        }

        public Path Clone() {
            var clonedPath = new Path();
            foreach (var edge in this)
            {
                clonedPath.Add(edge);
            }
            return clonedPath;
        }

        /// <summary>
        /// Determines whether or not this path currently contains any cycles that appear more than once.
        /// </summary>
        public bool ContainsDuplicateCycles() {
            var result = false;
            var cycles = FindCycles();
            if (cycles.Count > 0)
            {
                foreach (var cycle in cycles)
                {
                    if (ContainsInstancesOfSubset((Path)cycle) > 1)
                    {
                        result =  true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Determines if this path contains 1 or more instances of another path as a subset.
        /// Returns the count of instances discovered.
        /// </summary>
        public int ContainsInstancesOfSubset(Path subsetPath) {
            
            var numberOfSubsetInstancesFound = 0;
            
            //Special Case: length of path is greater than number of edges in collection.
            //  The collection cannot contain a subset greater than iteself.
            if (subsetPath.Count > this.Count)
            {
                return 0;
            }

            //Special Case: The supplied contains exactly two instances of the same edge.
            //  (Generally, results from a self referencing node)
            if (subsetPath.Count == 2 &&
                subsetPath[0].Equals(subsetPath[1]))
            {
                return 2;
            }

            var maxSupersetIndex = this.Count - 1;
            var maxSubsetIndex = subsetPath.Count - 1;
            for (var outerSupersetIndex = 0; outerSupersetIndex <= maxSupersetIndex; outerSupersetIndex++)
            {
                var subsetIndex = 0;
                for (var innerSupersetIndex = outerSupersetIndex; innerSupersetIndex <= innerSupersetIndex + subsetPath.Count; innerSupersetIndex++)
                {
                    if (innerSupersetIndex <= this.Count - 1)
                    {
                        var innerSupersetEdge = this[innerSupersetIndex];
                        var subsetEdge = subsetPath[subsetIndex];
                        if (innerSupersetEdge.Equals(subsetEdge))
                        {
                            subsetIndex++;
                            if (subsetIndex > maxSubsetIndex)
                            {
                                numberOfSubsetInstancesFound++;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return numberOfSubsetInstancesFound;
        }

        /// <summary>
        /// Finds all cycles within a the path and returns the result(s) as a pathcollection.
        /// </summary>
        public PathCollection FindCycles() {
            var foundCycles = new PathCollection();

            if (this.Count == 1)
            {
                if (this[0].FromNode.Equals(this[0].ToNode))
                {
                    var selfReferencingNodeCycle = new Path { this[0] };
                    foundCycles.Add(selfReferencingNodeCycle);
                }
            }
            else
            {
                var maxPossibleCycleLength = this.Count;
                var minPossibleCycleLength = 2;
                for (var cycleLength = minPossibleCycleLength; cycleLength <= maxPossibleCycleLength; cycleLength++)
                {
                    for (var cycleEndingIndex = cycleLength - 1; cycleEndingIndex <= this.Count - 1; cycleEndingIndex++)
                    {
                        var cycleStartingIndex = cycleEndingIndex - cycleLength + 1;
                        if (this[cycleStartingIndex].Equals(this[cycleEndingIndex]))
                        {
                            var cycle = new Path();
                            for (var i = cycleStartingIndex; i <= cycleEndingIndex; i++)
                            {
                                cycle.Add(this[i]);
                            }
                            if (!foundCycles.Contains(cycle))
                            {
                                foundCycles.Add(cycle);
                            }
                        }
                    }
                }
            }
            return foundCycles;
        }
        
        [DebuggerStepThrough]
        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as EdgeCollectionBase;
            if ((System.Object)p == null)
            {
                return false;
            }

            if (p.Count != Count)
            {
                return false;
            }

            var i = 0;
            foreach (var edge in this)
            {
                if (!edge.Equals(p[i]))
                {
                    return false;
                }
                i++;
            }

            return true;

        }

        [DebuggerStepThrough]
        public bool Equals(Path p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            var i = 0;
            foreach (var edge in this)
            {
                if (!edge.Equals(p[i]))
                {
                    return false;
                }
                i++;
            }

            return true;
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            return this.Count;
        }

        [DebuggerStepThrough]
        public static bool operator ==(Path a, Path b)
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

            var i = 0;
            foreach (var edge in a)
            {
                if (!edge.Equals(b[i]))
                {
                    return false;
                }
                i++;
            }

            return true;
        }

        [DebuggerStepThrough]
        public static bool operator !=(Path a, Path b)
        {
            return !(a == b);
        }

    }
}