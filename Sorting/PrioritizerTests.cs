using System.Collections.Generic;
using NUnit.Framework;

namespace SomeCompany.TicketPrioritizer.TestRunner
    {

    [TestFixture]
    public class PrioritizerTests
        {
            public Dictionary<int,string> TicketList {
                get
                    {
                        var ticketList = new Dictionary<int, string> {
                            {5, ""},
                            {1, ""},
                            {3, ""},
                            {2, ""}
                        };
                        return ticketList;
                    }
            }

            public Dictionary<int, string> LeftList
                {
                get
                    {
                    var ticketList = new Dictionary<int, string> {
                            {5, ""},
                            {1, ""}
                        };
                    return ticketList;
                    }
                }

            public Dictionary<int, string> RightList
                {
                get
                    {
                    var ticketList = new Dictionary<int, string> {
                            {3, ""},
                            {2, ""}
                        };
                    return ticketList;
                    }
                }
            
            [Test]
            public void MergeSort_Normally_Sorts()
                {
                    var p = new Prioritizer(TicketList);
                    p.TicketsReadyToCompare += TicketsReady_1;
                    var result = p.MergeSort_Tester();
                    Assert.AreEqual(new Dictionary<int, string> {{1, ""},
                                                                 {2, ""},
                                                                 {3, ""},
                                                                 {5, ""}}
                                   ,result);
                }

            private void TicketsReady_1(object sender, TicketsReadyToCompareEventArgs e)
                {
                    e.Comparison.ComparisonResult = ComparisonResult.NotSure;
                }

            [Test]
            public void Merge_Normally_Merges()
                {
                    var p = new Prioritizer(TicketList);
                    p.TicketsReadyToCompare += TicketsReady_2;
                    var result = p.Merge_Tester(LeftList, RightList);
                    Assert.AreEqual(TicketList,result);
                }

            private void TicketsReady_2(object sender, TicketsReadyToCompareEventArgs e)
                {
                e.Comparison.ComparisonResult = ComparisonResult.NotSure;
                }

        }
    }
