using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionsAdmin.Models
{
    [Serializable]
    public class QuizResult
    {

        public string Email { get; set; }

        public string Chapter { get; set; }

        public string CorrectAnsweredQuestions { get; set; }

        public string TotalQuestions { get; set; }

    }
}
