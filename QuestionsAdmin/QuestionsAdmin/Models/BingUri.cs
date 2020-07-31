using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionsAdmin.Models
{
    [Serializable]
    public class BingUri
    {
        public BingUri (string url)
        {
            this.url = url;
        }

        public string url { get; set; }
    }
}
