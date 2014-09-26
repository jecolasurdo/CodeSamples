using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SomeCompany.TicketPrioritizer
    {

    public class Prioritizer
        {

        private int _qn;
        private readonly DataTable _comparisonsDataTable;
        private readonly DataColumn _comparisonNumberDataColumn;
        private readonly DataColumn _itemChosenDataColumn;
        private readonly DataColumn _itemNotChosenDataColumn;
        private readonly DataColumn _questionAnsweredDataColumn;
        private int _totalComparisonsMade;

        /// <summary>
        /// Event raised once all tickets are prioritized and a result is ready.
        /// </summary>
        public event EventHandler<PrioritizedTicketsEventArgs> PrioritizationComplete;

        /// <summary>
        /// Event raised each time a pair of tickets is prepared and ready to be prioritized.
        /// </summary>
        public event EventHandler<TicketsReadyToCompareEventArgs> TicketsReadyToCompare;

        public Dictionary<int, string> TicketsToPrioritize{get; private set; }
        public Dictionary<int, string> PrioritizedTickets { get; private set; }
        public ulong MaxComparisonEstimate{ get; private set; }
        public ulong ComparisonsMadeByUser{ get; private set; }
            

        /// <param name="ticketsToPrioritize">A list of tickets (ticketId, summary/description) to be prioritized.</param>
        public Prioritizer(Dictionary<int, string> ticketsToPrioritize)
            {
                TicketsToPrioritize = ticketsToPrioritize;
                _qn = 1;
                _comparisonsDataTable = new DataTable();
                _comparisonNumberDataColumn = new DataColumn("Comparison Number");
                _itemChosenDataColumn = new DataColumn("Item Chosen");
                _itemNotChosenDataColumn = new DataColumn("Item Not Chosen");
                _questionAnsweredDataColumn = new DataColumn("Question Answered");
                _comparisonsDataTable.Columns.Add(_comparisonNumberDataColumn);
                _comparisonsDataTable.Columns.Add(_itemChosenDataColumn);
                _comparisonsDataTable.Columns.Add(_itemNotChosenDataColumn);
                _comparisonsDataTable.Columns.Add(_questionAnsweredDataColumn);
            }

        /// <summary>
        /// Initiates the prioritization process.
        /// </summary>
        public void Start()
            {
                var randomizedTickets = RandomizeTickets(TicketsToPrioritize);
                MaxComparisonEstimate = CalculateMaxComparisonEstimate(randomizedTickets.Count);
                var result = MergeSort(randomizedTickets);
                var e = new PrioritizedTicketsEventArgs(result,_comparisonsDataTable);
                PrioritizationComplete.Invoke(this, e);
            }

            /// <summary>
            /// Calculates the worst case scenario number of comparisons for merge-sort
            /// Based on http://en.wikipedia.org/wiki/Merge_sort#Analysis
            /// </summary>
            /// <param name="ticketCount"></param>
            /// <returns></returns>
            private ulong CalculateMaxComparisonEstimate(int ticketCount)
                {
                    var n = (ulong)ticketCount;
                    var r = (ulong)Math.Ceiling(Math.Log(n)/Math.Log(2));
                    return n * r - (2 ^ r) + 1;
                }

            private Dictionary<int, string> RandomizeTickets(Dictionary<int, string> ticketsToRandomize)
            {
            var r = new Dictionary<double, KeyValuePair<int, string>>();
            var random = new Random();
            foreach (KeyValuePair<int, string> t in ticketsToRandomize)
                {
                r.Add(random.NextDouble(), t);
                }
            var s = from entry in r
                    orderby entry.Key ascending
                    select entry;
            var result = new Dictionary<int, string>();
            foreach (var v in s.ToDictionary(pair => pair.Key, pair => pair.Value))
                {
                result.Add(v.Value.Key, v.Value.Value);
                }
            return result;
            }

        /// <summary>
        /// Implementation of merge_sort function as found at http://en.wikipedia.org/wiki/Merge_sort
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private Dictionary<int, string> MergeSort(Dictionary<int, string> m)
            {

            if (m.Count <= 1)
                {
                return m;
                }
                var left = new Dictionary<int, string>();
                var right = new Dictionary<int, string>();
                var middle = m.Count / 2;
                for (int i = 1; i <= m.Count; i++)
                {
                    var elementToAdd = m.ElementAt(i - 1);
                    if (i <= middle)
                    {
                        left.Add(elementToAdd.Key, elementToAdd.Value);
                    }
                    else
                    {
                        right.Add(elementToAdd.Key, elementToAdd.Value);
                    }
                }
                left = MergeSort(left);
                right = MergeSort(right);
                return Merge(left, right);
            }

        /// <summary>
        /// Implementation of merge function as found at http://en.wikipedia.org/wiki/Merge_sort
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private Dictionary<int, string> Merge(Dictionary<int, string> left, Dictionary<int, string> right)
            {
            var result = new Dictionary<int, string>();
            while (left.Count > 0 || right.Count > 0)
                {
                if (left.Count > 0 && right.Count > 0)
                    {
                    var ticketComparison = GetUserFeedback(left.First(), right.First());
                    if (ticketComparison.ComparisonResult == ComparisonResult.RightIsGreater)
                        {
                        AddResultToComparisonDatatable(ticketComparison.RightTicket.Value,
                                                       ticketComparison.LeftTicket.Value,
                                                       ticketComparison.Question.Value);
                        result.Add(left.First().Key, left.First().Value);
                        left.Remove(left.First().Key);
                        }
                    else
                        {
                        AddResultToComparisonDatatable(ticketComparison.LeftTicket.Value,
                               ticketComparison.RightTicket.Value,
                               ticketComparison.Question.Value);
                        result.Add(right.First().Key, right.First().Value);
                        right.Remove(right.First().Key);
                        }
                    }
                else if (left.Count > 0)
                    {
                    AddResultToComparisonDatatable(left.First().Value,"N/A","N/A");
                    result.Add(left.First().Key, left.First().Value);
                    left.Remove(left.First().Key);
                    }
                else if (right.Count > 0)
                    {
                    AddResultToComparisonDatatable(right.First().Value,"N/A","N/A");                               
                    result.Add(right.First().Key, right.First().Value);
                    right.Remove(right.First().Key);
                    }
                }
            return result;
            }

        private TicketComparison GetUserFeedback(KeyValuePair<int, string> left, KeyValuePair<int, string> right)
            {
                var result = new TicketComparison();    
                var ticketComparison = new TicketComparison();
                ticketComparison.ComparisonResult = ComparisonResult.NotSet;
                ticketComparison.LeftTicket = left;
                ticketComparison.RightTicket = right;
                while (ticketComparison.ComparisonResult == ComparisonResult.NotSet ||
                       ticketComparison.ComparisonResult == ComparisonResult.NotSure)
                {
                    ticketComparison.Question = new KeyValuePair<int, string>(_qn, QuestionList.Questions[_qn]);
                    var e = new TicketsReadyToCompareEventArgs(ticketComparison, MaxComparisonEstimate, ComparisonsMadeByUser);
                    if (TicketsReadyToCompare != null)
                    {
                        TicketsReadyToCompare.Invoke(this, e);
                    }
                    while (e.Comparison.ComparisonResult == ComparisonResult.NotSet)
                    {
                        // do nothing and wait for another thread to change the comparison result value.
                    }
                    if (e.Comparison.ComparisonResult == ComparisonResult.NotSure)
                    {
                        _qn++;
                        if (_qn > QuestionList.MaxQuestionNumber)
                        {
                            e.Comparison.ComparisonResult = left.Key >= right.Key
                                                                ? ComparisonResult.LeftIsGreater
                                                                : ComparisonResult.RightIsGreater;
                            result = e.Comparison;
                            break;
                        }
                        e.Comparison.ComparisonResult = ComparisonResult.NotSet;
                    }
                    else
                    {
                        ComparisonsMadeByUser++;
                        result = e.Comparison;
                        break;
                    }
                }
                _qn = 1;
                return result;
            }

            private void AddResultToComparisonDatatable(string itemChosen,
                                                        string itemNotChosen,
                                                        string questionAnswered)
                {
                    object[] rowItems = {++_totalComparisonsMade,itemChosen, itemNotChosen, questionAnswered};
                    _comparisonsDataTable.Rows.Add(rowItems);
                }


#if DEBUG
        public Dictionary<int, string> MergeSort_Tester()
            {
            return MergeSort(TicketsToPrioritize);
            }

        public Dictionary<int, string> Merge_Tester(Dictionary<int, string> left, Dictionary<int, string> right)
            {
            return Merge(left, right);
            }
#endif
        }
    }
