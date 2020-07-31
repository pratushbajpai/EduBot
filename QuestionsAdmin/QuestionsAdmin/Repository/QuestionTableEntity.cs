using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace QuestionsAdmin.Repository
{
    public class QuestionTableEntity:TableEntity
    {
        public int TotalQuestions { get; set; }

        public DateTime LastTestTakenTime { get; set; }

        public int CorrectAnsweredQuestions { get; set; }
    }
}
