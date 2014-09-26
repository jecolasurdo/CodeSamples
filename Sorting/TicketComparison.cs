using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomeCompany.TicketPrioritizer
    {

    public class TicketComparison
        {
        public KeyValuePair<int, string> LeftTicket;
        public KeyValuePair<int, string> RightTicket;
        public ComparisonResult ComparisonResult {get; set; }
        public KeyValuePair<int,string> Question {get; set;}
    }
}
