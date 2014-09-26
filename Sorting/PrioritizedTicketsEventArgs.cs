using System;
using System.Collections.Generic;
using System.Data;

namespace SomeCompany.TicketPrioritizer
    {
    public class PrioritizedTicketsEventArgs : EventArgs
        {

        public PrioritizedTicketsEventArgs(Dictionary<int, string> prioritizedTickets, DataTable comparisons)
            {
                PrioritizedTickets = prioritizedTickets;
                Comparisons = comparisons;
            }

        public Dictionary<int, string> PrioritizedTickets
            {
            get;
            private set;
            }

        public DataTable Comparisons
            {
            get;
            private set;
            }

        }
    }