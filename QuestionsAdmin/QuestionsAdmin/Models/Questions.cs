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

        public Questions()
        {
            QuestionsList = new List<Question>();
        }
    }
}
