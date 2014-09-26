using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;
using SomeCompany.Pathfinder.GraphObjects;

namespace PathFinder.UseCases
{
    [TestFixture]
    public class Cases
    {
        [Test]
        public void FindStartNodes_SelfReferencingNode_DoesntFindsNode() {
            var node1 = new Node(1, "1");
            var e1 = new Edge(1, "A", node1, node1);
            var ec = new EdgeCollection { e1 };
            var actualResult = ec.FindStartNodes();
            Assert.IsEmpty(actualResult);
        }

        [Test]
        public void FindStartNodes_StartNodePresent_FindsNode() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var e1 = new Edge(1, "e1", node1, node2);
            var e2 = new Edge(2, "e2", node1, node3);
            var ec = new EdgeCollection {e1, e2};
            var actualResult = ec.FindStartNodes();

            var expectedResult = new Stack<INode>();
            expectedResult.Push(node1);

            Assert.AreEqual((INode)actualResult.Pop(), (INode)expectedResult.Pop());
        }

        [Test]
        public void FindStartNodes_TwoStartNodesPresent_FindsNodes()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var e1 = new Edge(1, "e1", node1, node2);
            var e2 = new Edge(2, "e2", node1, node3);
            var e3 = new Edge(3, "e3", node4, node3);
            var ec = new EdgeCollection { e1, e2, e3};
            var actualResult = ec.FindStartNodes();

            var expectedResult = new Stack<INode>();
            expectedResult.Push(node1);
            expectedResult.Push(node4);

            if (actualResult.Contains(node1) &&
                actualResult.Contains(node4) &&
                actualResult.Count == 2)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail("Expected to find only nodes 1 and 4, but did not.");
            }
        }

        [Test]
        public void NodeEquality_NodeIdsNotEqual_AreNotEqual() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            Assert.AreNotEqual(node1,node2);
        }

        [Test]
        public void NodeEquality_NodesIdsEqual_AreEqual()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(1, "2");
            Assert.AreEqual(node1, node2);
        }

        [Test]
        public void PathContainsEdge_Normally_FindsEdge()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var edgeA = new Edge(1, "A", node1, node2);
            var p = new Path { edgeA };
            Assert.IsTrue(p.Contains(edgeA));
        }

        [Test]
        public void EdgeEquality_EdgesSameId_AreEqual()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var edgeA = new Edge(1, "A", node1, node2);
            var edgeB = new Edge(1, "A", node1, node2);
            Assert.AreEqual(edgeA, edgeB);
        }


        //  1  
        //  | 
        //  |A 
        //  v 
        //  2
        //  |
        //  |C
        //  v
        //  4
        // Expected Paths: AC
        [Test]
        public void FindAllPaths_SimplePath_FindsPath() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node4 = new Node(4, "4");
            var edgeA = new Edge(1, "A", node1, node2);
            var edgeC = new Edge(3, "C", node2, node4);
            var ec = new EdgeCollection { edgeA, edgeC};
            var actualPathCollection = ec.FindAllPaths();

            var path1 = new Path { edgeA, edgeC };
            var expectedPathCollection = new PathCollection { path1 };

            CollectionAssert.AreEquivalent(expectedPathCollection, actualPathCollection);
        }


        //  1       6 
        //  |       | 
        //  |A      |E
        //  v   B   v 
        //  2------>3 
        //  |       | 
        //  |C      |D
        //  v       v 
        //  4       5 
        // Expected Paths: AC, ABD, ED
        [Test(Description = "This assumes a directed acrlic graph with two discrete start points" +
                            "and a two discrete end points. The function should be able to identify" +
                            "three paths through the defined edges.")]
        public void FindAllPaths_DirectedAcrylicGraph_FindsPaths() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var node6 = new Node(6, "6");
            var edgeA = new Edge(1, "A", node1, node2);
            var edgeB = new Edge(2, "B", node2, node3);
            var edgeC = new Edge(3, "C", node2, node4);
            var edgeD = new Edge(4, "D", node3, node5);
            var edgeE = new Edge(5, "E", node6, node3);
            var ec = new EdgeCollection {edgeA, edgeB, edgeC, edgeD, edgeE};
            var actualPathCollection = ec.FindAllPaths();

            var path1 = new Path {edgeA, edgeC};
            var path2 = new Path {edgeA, edgeB, edgeD};
            var path3 = new Path {edgeE, edgeD};
            var expectedPathCollection = new PathCollection {path1, path2, path3};

            CollectionAssert.AreEquivalent(expectedPathCollection, actualPathCollection);
        }


        //  1        
        //  |        
        //  |A       
        //  v        
        //  2<------+
        //  |       |
        //  |B      |D
        //  v   C   |
        //  3------>4
        //          |
        //          |E
        //          v
        //          5
        // Expected Paths: ABCE, ABCDBCE
        [Test(Description="This assumes a graph with a discrete start point and end point," +
                          "but contains a single strong component relationship." +
                          "The method should show 1 iteration through each strong " +
                          "component as a distinct path.")]
        public void FindAllPaths_ContainsStrongComponent_FindsPaths() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var edgeA = new Edge(1, "A", node1, node2);
            var edgeB = new Edge(2, "B", node2, node3);
            var edgeC = new Edge(3, "C", node3, node4);
            var edgeD = new Edge(4, "D", node4, node2);
            var edgeE = new Edge(5, "E", node4, node5);
            var ec = new EdgeCollection { edgeA, edgeB, edgeC, edgeD, edgeE};
            var actualPathCollection = ec.FindAllPaths();

            var path1 = new Path{edgeA, edgeB, edgeC, edgeE};
            var path2 = new Path {edgeA, edgeB, edgeC, edgeD, edgeB, edgeC, edgeE};
            var expectedPathCollection = new PathCollection {path1, path2};

            CollectionAssert.AreEquivalent(expectedPathCollection, actualPathCollection);

        }

        //  1                                    
        //  |                                    
        //  |A                                   
        //  v                                    
        //  2<------+<-------+                   
        //  |       |        |                   
        //  |B      |D       |G                  
        //  v       |        |                   
        //  3------>4------->6                   
        //      C   |    F                       
        //          |E                           
        //          v                            
        //          5         
        //Expected Paths: ABCE, ABCDBCE, ABCFGBCE, ABCDBCFGBCE, ABCFGBCDBCE           
        [Test]
        public void FindAllPaths_ContainsAdjacesntStrongComponents_FindsPaths() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var node6 = new Node(6, "6");
            var A = new Edge(1, "A", node1, node2);
            var B = new Edge(2, "B", node2, node3);
            var C = new Edge(3, "C", node3, node4);
            var D = new Edge(4, "D", node4, node2);
            var E = new Edge(5, "E", node4, node5);
            var F = new Edge(6, "F", node4, node6);
            var G = new Edge(7, "G", node6, node2);
            var ec = new EdgeCollection {A, B, C, D, E, F, G};
            var actualPathCollection = ec.FindAllPaths();

            var p1 = new Path {A, B, C, E};
            var p2 = new Path {A, B, C, D, B, C, E};
            var p3 = new Path {A, B, C, F, G, B, C, E};
            var p4 = new Path {A, B, C, D, B, C, F, G, B, C, E};
            var p5 = new Path {A, B, C, F, G, B, C, D, B, C, E};
            var expectedPathCollection = new PathCollection {p1, p2, p3, p4, p5};
            
            CollectionAssert.AreEquivalent(expectedPathCollection, actualPathCollection);
        }

        //   1
        //   |        
        //   |        
        //  A|        
        //   |        
        //   v        
        //   2<-----+ 
        //   |      | 
        //   |      | 
        //  B|      |D
        //   |      | 
        //   v      + 
        //   3----->4 
        //   |   C   
        //   |        
        //  E|        
        //   |        
        //   v        
        //   5<-----+ 
        //   |      | 
        //   |      | 
        //  F|      |H
        //   |      | 
        //   v      + 
        //   6----->7 
        //   |   G    
        //   |        
        //  I|        
        //   |        
        //   v        
        //   8        
        [Test]
        public void FindAllPaths_MultipleNonAdjacentStrongComponents_FindsPaths() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var node6 = new Node(6, "6");
            var node7 = new Node(7, "7");
            var node8 = new Node(8, "8");
            var A = new Edge(1, "A", node1, node2);
            var B = new Edge(2, "B", node2, node3);
            var C = new Edge(3, "C", node3, node4);
            var D = new Edge(4, "D", node4, node2);
            var E = new Edge(5, "E", node3, node5);
            var F = new Edge(6, "F", node5, node6);
            var G = new Edge(7, "G", node6, node7);
            var H = new Edge(8, "H", node7, node5);
            var I = new Edge(9, "I", node6, node8);
            var ec = new EdgeCollection {A, B, C, D, E, F, G, H, I};
            var p1 = new Path {A, B, E, F, I};
            var p2 = new Path {A, B, C, D, B, E, F, I};
            var p3 = new Path {A, B, E, F, G, H, F, I};
            var p4 = new Path {A, B, C, D, B, E, F, G, H, F, I};
            var expectedPathCollection = new PathCollection {p1, p2, p3, p4};
            var actualPathCollection = ec.FindAllPaths();
            CollectionAssert.AreEquivalent(expectedPathCollection,actualPathCollection);
        }

        [Test]
        public void FindAllPaths_SimpleSelfReferencingNode_FindsPaths()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var A = new Edge(1, "A", node1, node2);
            var B = new Edge(2, "B", node2, node2);
            var ec = new EdgeCollection { A, B };
            var actualPathCollection = ec.FindAllPaths();

            var p1 = new Path { A, B };
            var expectedPathCollection = new PathCollection { p1 };

            CollectionAssert.AreEquivalent(expectedPathCollection, actualPathCollection);
        }

        [Test]
        public void FindAllPaths_ComplexSelfReferencingNode_FindsPaths() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var A = new Edge(1, "A", node1, node2);
            var B = new Edge(2, "B", node2, node2);
            var C = new Edge(3, "C", node2, node3);
            var ec = new EdgeCollection {A, B, C};
            var p1 = new Path {A, C};
            var p2 = new Path {A, B, C};
            var expectedPathCollection = new PathCollection {p1, p2};
            var actualPathCollection = ec.FindAllPaths();
            CollectionAssert.AreEquivalent(expectedPathCollection,actualPathCollection);
        }
 
        public void FindAllPaths_NoDistinctEntryPoint_NotSureWhatItShouldDo() {
        }

        public void FindAllPaths_NoDistinctExitPoint_NotSureWhatItShouldDo() {
            
        }

        [Test]
        public void IsTerminal_SingleEdgePathIsTerminal_ReturnsTrue() {
            var node1 = new Node(1, "1");
            var edge1 = new Edge(1, "1", node1, null);
            var ec = new EdgeCollection {edge1};
            var path1 = new Path {edge1};
            Assert.IsTrue(ec.IsTerminal(path1));
        }

        [Test]
        public void IsTerminal_SimpleMultiEdgePathIsTerminal_ReturnsTrue() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var edge1 = new Edge(1, "A", node1, node2);
            var edge2 = new Edge(2, "B", node2, node3);
            var ec = new EdgeCollection {edge1, edge2};
            var path = new Path {edge1, edge2};
            Assert.IsTrue(ec.IsTerminal(path));
        }

        [Test]
        public void IsTerminal_PathIsNotTerminal_ReturnsFalse()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var edge1 = new Edge(1, "A", node1, node2);
            var edge2 = new Edge(2, "B", node2, node3);
            var ec = new EdgeCollection { edge1, edge2 };
            var path = new Path { edge1 };
            Assert.IsFalse (ec.IsTerminal(path));
        }

        [Test]
        public void IsTerminal_CircularSingleEdge_ReturnsTrue() {
            var node1 = new Node(1, "1");
            var edge1 = new Edge(1, "A", node1, node1);
            var ec = new EdgeCollection { edge1 };
            var path = new Path { edge1 };
            Assert.IsTrue(ec.IsTerminal(path));
        }

        [Test]
        public void ContainsDuplicateCycle_RepeatedEdge_FindsCycles() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var A = new Edge(1, "A", node1, node2);
            var B = new Edge(2, "B", node2, node2);
            var path = new Path { A, B, B };
            Assert.IsTrue(path.ContainsDuplicateCycles());
        }

        [Test]
        public void ContainsDuplicateCycle_CyclesNotOverlapping_FindsCycles() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var node6 = new Node(6, "6");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var E3 = new Edge(3, "3", node4, node2);
            var E4 = new Edge(4, "4", node3, node5);
            var E5 = new Edge(5, "5", node5, node6);
            var path = new Path { E1, E2, E3, E1, E4, E5, E1, E2, E3, E1 };
            Assert.IsTrue(path.ContainsDuplicateCycles());
        }

        [Test]
        public void ContainsDuplicateCycle_CyclesOverlapping_FindsCycles()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var E3 = new Edge(3, "3", node4, node2);
            var path = new Path { E1, E2, E3, E1, E2, E3, E1 };
            Assert.IsTrue(path.ContainsDuplicateCycles());
        }

        [Test]
        public void ContainsDuplicateCycle_NearlyCycle_FindsNoCycles()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var E3 = new Edge(3, "3", node4, node2);
            var E4 = new Edge(4, "4", node3, node5);
            var path = new Path { E1, E2, E3, E1, E4, E1, E2, E3 };
            Assert.IsFalse(path.ContainsDuplicateCycles());
        }

        [Test]
        public void ContainsInstancesOfSubset_SingleSubset_ReturnsOne() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var E3 = new Edge(3, "3", node4, node2);
            var E4 = new Edge(4, "4", node3, node5);
            var supersetPath = new Path { E1, E2, E3, E1, E4, E1, E2, E3 };
            var subsetPath = new Path { E3, E1, E4 };
            var actualNumberOfSubsetsFound = supersetPath.ContainsInstancesOfSubset(subsetPath);
            var expectedNumberOfSubsetsFound = 1;
            Assert.AreEqual(expectedNumberOfSubsetsFound, actualNumberOfSubsetsFound);
        }

        [Test]
        public void ContainsInstancesOfSubset_TwoNonConsecutiveSubsets_ReturnsTwo()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var E3 = new Edge(3, "3", node4, node2);
            var E4 = new Edge(4, "4", node3, node5);
            var supersetPath = new Path { E1, E2, E3, E1, E4, E1, E2, E3 };
            var subsetPath = new Path { E1, E2, E3 };
            var actualNumberOfSubsetsFound = supersetPath.ContainsInstancesOfSubset(subsetPath);
            var expectedNumberOfSubsetsFound = 2;
            Assert.AreEqual(expectedNumberOfSubsetsFound, actualNumberOfSubsetsFound);
        }

        [Test]
        public void ContainsInstancesOfSubset_TwoConsecutiveSubsets_ReturnsTwo()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var E3 = new Edge(3, "3", node4, node2);
            var E4 = new Edge(4, "4", node3, node5);
            var supersetPath = new Path { E1, E2, E3, E1, E2, E3 };
            var subsetPath = new Path { E1, E2, E3 };
            var actualNumberOfSubsetsFound = supersetPath.ContainsInstancesOfSubset(subsetPath);
            var expectedNumberOfSubsetsFound = 2;
            Assert.AreEqual(expectedNumberOfSubsetsFound, actualNumberOfSubsetsFound);
        }

        [Test]
        public void ContainsInstancesOfSubset_TwoOverLappedSubsets_ReturnsTwo()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var E3 = new Edge(3, "3", node4, node2);
            var E4 = new Edge(4, "4", node3, node5);
            var supersetPath = new Path { E1, E2, E3, E1, E2, E3, E1 };
            var subsetPath = new Path { E1, E2, E3, E1 };
            var actualNumberOfSubsetsFound = supersetPath.ContainsInstancesOfSubset(subsetPath);
            var expectedNumberOfSubsetsFound = 2;
            Assert.AreEqual(expectedNumberOfSubsetsFound, actualNumberOfSubsetsFound);
        }

        [Test]
        public void PathEquality_PathLengthsNotEqual_ReturnsNotEqual() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var path1 = new Path { E1, E2, E1, E2 };
            var path2 = new Path { E1, E2, E1 };
            Assert.AreNotEqual(path1,path2);
        }

        [Test]
        public void PathEquality_PathsEqual_ReturnsEqual() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var path1 = new Path {E1, E2, E1, E2};
            var path2 = new Path {E1, E2, E1, E2};
            Assert.IsTrue(path1.Equals(path2));
        }

        [Test]
        public void PathCollectionContains_ContainsPath_ReturnsTrue()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var path1 = new Path { E1, E2 };
            var path2 = new Path { E1, E2, E1};
            var pathWithSameValueAsPath1 = new Path { E1, E2 };
            var pc = new PathCollection {path1, path2};
            Assert.IsTrue(pc.Contains(pathWithSameValueAsPath1));
        }

        [Test]
        public void PathEquality_PathEdgesNotEqual_ReturnsNotEqual()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var E3 = new Edge(3, "3", node2, node1);
            var path1 = new Path { E1, E2, E1, E2 };
            var path2 = new Path { E1, E2, E1, E3 };
            Assert.IsFalse(path1.Equals(path2));
        }

        [Test]
        public void FindCycles_OneCyclePresent_FindsCycle() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var E3 = new Edge(3, "3", node4, node2);
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var path = new Path { E1, E2, E1, E3 };
            
            var cycle1 = new Path { E1, E2, E1 };
            var expectedCycles = new PathCollection {cycle1};
            var actualCycles = path.FindCycles();
            CollectionAssert.AreEquivalent(expectedCycles, actualCycles);
        }

        [Test]
        public void FindCycles_TwoNonAdjacentCyclesPresent_FindsCycles()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var node5 = new Node(5, "5");
            var node6 = new Node(6, "6");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var E3 = new Edge(3, "3", node4, node2);
            var E4 = new Edge(4, "4", node3, node5);
            var E5 = new Edge(5, "5", node5, node6);
            var path = new Path { E1, E2, E1, E3, E4, E5, E4 };

            var cycle1 = new Path { E1, E2, E1 };
            var cycle2 = new Path { E4, E5, E4 };
            var expectedCycles = new PathCollection { cycle1, cycle2 };
            var actualCycles = path.FindCycles();
            CollectionAssert.AreEquivalent(expectedCycles, actualCycles);
        }

        [Test]
        public void FindCycles_TwoOverlappingCyclesPresent_FindsCycles()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var node4 = new Node(4, "4");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var E3 = new Edge(3, "3", node4, node2);
            var path = new Path { E1, E2, E1, E3, E1 };

            var cycle1 = new Path { E1, E2, E1 };
            var cycle2 = new Path { E1, E3, E1 };
            var cycle3 = new Path { E1, E2, E1, E3, E1 };
            var expectedCycles = new PathCollection { cycle1, cycle2, cycle3 };
            var actualCycles = path.FindCycles();
            CollectionAssert.AreEquivalent(expectedCycles, actualCycles);
        }

        [Test]
        public void FindCycles_NoCyclesPresent_FindsNoCycles()
        {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var path = new Path { E1, E2 };

            var cycles = path.FindCycles();
            CollectionAssert.IsEmpty(cycles);
        }

        [Test]
        public void FindCycles_PathContainsSingleEdge_FindsNoCycles() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var E1 = new Edge(1, "1", node1, node2);
            var path = new Path { E1 };
            var cycles = path.FindCycles();
            CollectionAssert.IsEmpty(cycles);
        }

        [Test]
        public void FindCycles_CycleIsLengthOfPath_FindsCycle() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var path = new Path { E1, E2, E1 };

            var cycle = new Path { E1, E2, E1 };
            var expectedCycles = new PathCollection { cycle };
            var actualCycles = path.FindCycles();
            CollectionAssert.AreEquivalent(expectedCycles, actualCycles);
        }

        [Test]
        public void FindCycles_DuplicateCyclesFound_OnlyReturnsDistinctCycles() {
            var node1 = new Node(1, "1");
            var node2 = new Node(2, "2");
            var node3 = new Node(3, "3");
            var E1 = new Edge(1, "1", node1, node2);
            var E2 = new Edge(2, "2", node2, node3);
            var path = new Path { E1, E2, E1, E2, E1 };

            var cycle1 = new Path {E1, E2, E1};
            var cycle2 = new Path {E2, E1, E2};
            var cycle3 = new Path {E1, E2, E1, E2, E1};
            var expectedCycles = new PathCollection { cycle1, cycle2, cycle3 };
            var actualCycles = path.FindCycles();
            CollectionAssert.AreEquivalent(expectedCycles, actualCycles);
        }

        [Test(Description = "This tests that a self referencing node is classified as a cycle.")]
        public void FindCycles_SelfReferencingNode_FindsCycle()
        {
            var node1 = new Node(1, "1");
            var E1 = new Edge(1, "1", node1, node1);
            var path = new Path { E1 };

            var cycle1 = new Path { E1 };
            var expectedCycles = new PathCollection { cycle1 };
            var actualCycles = path.FindCycles();
            CollectionAssert.AreEquivalent(expectedCycles, actualCycles);
        }

        [Test(Description = "An exception should be thrown if an edge is added wherein edge's to node id" +
                            "matches the id of an existing node in the collection, but has a different node name" +
                            "than the existing.")]
        public void EdgeCollection_AddSameToNodeIdDifferentName_ThrowsException() {
            var n1 = new Node(1, "1");
            var n2 = new Node(2, "2");
            var n2A = new Node(2, "2A");
            var e1 = new Edge(1,"E1", n1, n2);
            var e2 = new Edge(2, "n2", n1, n2A);
            var ec = new EdgeCollection {e1};
            Assert.Throws<Exception>(() => ec.Add(e2));
        }

        [Test(Description = "An exception should be thrown if an edge is added wherein the edge's from node id" +
                    "matches the id of an existing node in the collection, but has a different node name" +
                    "than the existing.")]
        public void EdgeCollection_AddSameFromNodeIdDifferentName_ThrowsException()
        {
            var n1 = new Node(1, "1");
            var n2 = new Node(2, "2");
            var n1A = new Node(1, "1A");
            var e1 = new Edge(1, "E1", n1, n2);
            var e2 = new Edge(2, "n2", n1A, n2);
            var ec = new EdgeCollection { e1 };
            Assert.Throws<Exception>(() => ec.Add(e2));
        }

        [Test]
        public void TestDecisionTree() {
            var n1 = new Node(1, "Checking eligibility for a team member.");
            var n2 = new Node(2, "Active Employee?");
            var n3 = new Node(3, "Currently Enrolled?");
            var n4 = new Node(4, "Rehire?");
            var n5 = new Node(5, "Previously Enrolled?");
            var n6 = new Node(6, "Eligible for targetted enrollment date in a past year?");
            var n7 = new Node(7, "Total calendar days of service by enrollment date?");
            var n8 = new Node(8, "Age by enrollment Date?");
            var n9 = new Node(9, "Targeted enrollment date?");
            var n10 = new Node(10, "Total hours worked since Jan 1st?");
            var n11 = new Node(11, "Projected pre-enrollment date hours?");
            var n12 = new Node(12, "Anniversary date between Jan 1 and July 1?");
            var n13 = new Node(13, "First anniversary is this year?");
            var n14 = new Node(14, "Anniversary date has passed?");
            var n15 = new Node(15, "Projected hours by anniversary date?");
            var n16 = new Node(16, "Hours worked prior to anniversary?");
            var n17 = new Node(17, "Immediately eligible to enroll.");
            var n18 = new Node(18, "Eligible to enroll.");
            var n19 = new Node(19, "Not eligible to enroll.");
            var n20 = new Node(20, "Potentially eligible to enroll.");

            var ec = new EdgeCollection
            {
                {"", n1, n2},
                {"No", n2, n19},
                {"Yes", n2, n3},
                {"Currently Enrolled", n3, n19},
                {"Not Enrolled", n3, n4},
                {"No", n4, n7},
                {"Yes", n4, n5},
                {"No", n5, n6},
                {"Yes", n6, n17},
                {"Yes", n5, n17},
                {"Yes", n6, n17},
                {"No", n6, n7},
                {"<365", n7, n19},
                {">=364", n7, n8},
                {"<20", n8, n19},
                {">=20", n8, n9},
                {"January 1st", n9, n10},
                {"<1000", n10, n11},
                {"<1000", n11, n19},
                {"July 1st", n9, n12},
                {">=1000", n10, n18},
                {">=1000", n11, n20},
                {"No", n12, n19},
                {"Yes", n12, n13},
                {"No", n13, n19},
                {"Yes", n13, n14},
                {"No", n14, n15},
                {"<1000", n15, n19},
                {">=1000", n15, n20},
                {"Yes", n14, n16},
                {"<1000", n16, n19},
                {">=1000", n16, n18}
            };

            var pc = new PathCollection();
            pc = (PathCollection)ec.FindAllPaths();
            var allPathDescriptions = pc.DescribeAllPaths();
            Assert.IsNotNullOrEmpty(allPathDescriptions);
        }
    }
}
