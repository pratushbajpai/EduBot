using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionsAdmin.Models
{
    [Serializable]
    public class Questions
    {
        public List<Question> QuestionsList { get; set; }

        public int TotalQuestions { get; set; }

        public int CorrectAnsweredQuestions { get; set; }

        public string LastTakenRelativeTime { get; set; }

        public bool LastTaken { get; set; }

        public Questions()
        {
            QuestionsList = new List<Question>();
        }
    }
}
