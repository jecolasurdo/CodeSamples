using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public class EdgeCollection : EdgeCollectionBase, IEdgeCollection
    {
        /// <summary>
        /// Finds all paths through the collection of edges.
        /// Assumes that the graph has one or more start nodes, defined as a node with outbound paths, but no inbound paths.
        /// </summary>
        public PathCollection FindAllPaths() {
            var paths = new PathCollection();

            var startNodes = FindStartNodes();
            foreach (var startNode in startNodes)
            {
                var unexploredPaths = new Stack<IPath>();
                var initialEdges = FindOutboundEdgesForNode(startNode);
                foreach (var initialEdge in initialEdges)
                {
                    var initialPath = new Path { initialEdge };
                    unexploredPaths.Push(initialPath);
                    while (unexploredPaths.Count > 0)
                    {
                        var currentPath = unexploredPaths.Pop();
                        //look at outedges from the end of this path
                        var outEdges = FindOutboundEdgesForNode(currentPath.FinalNode);
                        //choose first outEdge and add to working path
                        var workingPath = currentPath.Clone();
                        if (outEdges.Count != 0)
                        {
                            var firstOutEdge = outEdges.Pop();
                            workingPath.Add(firstOutEdge);

                            //add each remainig outEdge to path and push into unexploredPaths stack
                            while (outEdges.Count > 0)
                            {
                                var unexploredPath = currentPath.Clone();
                                var outEdge = outEdges.Pop();
                                if (!unexploredPath.Contains(outEdge))
                                {
                                    unexploredPath.Add(outEdge);
                                    unexploredPaths.Push(unexploredPath);
                                }
                            }
                        }

                        //check to see if working path contains any cycles.
                        //check to see if working path contains any duplicate cycles. If so, abandon it and move on to next unexplored path
                        if (workingPath.ContainsDuplicateCycles())
                        {
                            continue;
                        }

                        //if no duplicate cycles found, check to see if this path is terminal.
                        if (IsTerminal(workingPath))
                        {
                            //If it is, add it to the path collection and move on.
                            paths.Add(workingPath);
                        }
                        else
                        {
                            //If it is not, add it to the unexploredPaths stack and move on
                            unexploredPaths.Push(workingPath);
                        }
                    }
                }
            }
            return paths;
        }
        
        public Stack<INode> FindStartNodes() {
            var startNodes = new Stack<INode>();
            var fromNodes = new List<INode>();
            var toNodes = new List<INode> ();

            foreach (var edge in this)
            {
                fromNodes.Add(edge.FromNode);
                toNodes.Add(edge.ToNode);
            }

            foreach (var fromNode in fromNodes)
            {
                if (!toNodes.Contains(fromNode) && !startNodes.Contains(fromNode))
                {
                    startNodes.Push(fromNode);
                }
            }
            return startNodes;
        }

        public Stack<IEdge> FindOutboundEdgesForNode(INode node) {
            var outboundEdges = new Stack<IEdge>();
            foreach (var edge in this)
            {
                if (edge.FromNode.Equals(node))
                {
                    outboundEdges.Push(edge);
                }
            }
            return outboundEdges;
        }

        /// <summary>
        /// Reflects whether or not this path is terminal.
        /// A terminal path is one where the last edge in the path has a null to-node.
        /// </summary>
        /// <returns></returns>
        public bool IsTerminal(Path pathToCheck) {
            if (pathToCheck.FinalNode == null)
            {
                return true;
            }
            
            if (pathToCheck.Last().ToNode.Equals(pathToCheck.Last().FromNode))
            {
                return true;
            }
            var outEdges = FindOutboundEdgesForNode(pathToCheck.FinalNode);
            return outEdges.Count == 0;
        }

    }
}
