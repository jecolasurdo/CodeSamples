using System;

namespace SomeCompany.TicketPrioritizer
    {
    public class TicketsReadyToCompareEventArgs : EventArgs
        {
        public TicketsReadyToCompareEventArgs(TicketComparison ticketComparison, ulong maxComparisonEstimate, ulong totalComparisonsMade)
            {
            Comparison = ticketComparison;
            MaxComparisonEstimate = maxComparisonEstimate;
            TotalComparisonsMade = totalComparisonsMade;
            }

        public TicketComparison Comparison
            {
            get;
            set;
            }
        public ulong MaxComparisonEstimate
            {
            get;
            private set;
            }
        public ulong TotalComparisonsMade
            {
            get;
            private set;
            }
        }
    }