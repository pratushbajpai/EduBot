using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionsAdmin.Models
{
    [Serializable]
    public class Question
    {
        public string Id { get; set; }

        public string Prompt { get; set; }

        public string OptionValues { get; set; }

        public List<String> Options 
        { 
            get
            {
                var options = new List<String>();
                if(!String.IsNullOrWhiteSpace(OptionValues))
                {
                    options = OptionValues?.Split("|")?.ToList();
                }

                return options;
            }
        }

        public int AnswerIndex { get; set; } 
    }
}
