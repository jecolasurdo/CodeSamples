using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomeCompany.TicketPrioritizer
    {

    /// <summary>
    /// Contains an enumerated list of questions that are fed into the TicketComparison object by the Prioritizer class
    /// </summary>
    public static class QuestionList
        {
        static public Dictionary<int, string> Questions
            {
            get
                {
                    var result = new Dictionary<int, string>();
                    var i = 0;
                    var questionString = Properties.Settings.Default.differentiationQuestions.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var question in questionString)
                    {
                        i++;
                        result.Add(i,question);
                    }
                    return result;
                }
            }

            public static int MaxQuestionNumber
                {
                get { return Questions.Count; }
                }
        }
}
