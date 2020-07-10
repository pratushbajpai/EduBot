using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotResourcesAPI.Contracts
{
    [Serializable]
    public class Question
    {
        public string Prompt { get; set; }

        public List<String> Options { get; set; }
        
        public Question()
        {
            Options = new List<String>();
        }

        public int AnswerIndex { get; set; } 
    }
}
