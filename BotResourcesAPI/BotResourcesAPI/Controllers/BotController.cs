using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotResourcesAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BotResourcesAPI.Controllers
{
    [Route("api/[controller]")]
    public class BotController : Controller
    {
        // GET: api/<BotController>
        [HttpGet]
        public Questions Get()
        {
            return GetQuestions();
        }

        // GET api/<BotController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BotController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BotController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BotController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public Questions GetQuestions()
        {
            Questions questions = new Questions()
            {
                QuestionsList = new List<Question>()
               {
                  new Question()
                  {
                      Prompt = "Who is gallelio?",
                      AnswerIndex = 0,
                      Options = new List<string>()
                      {
                          "Astrologer",
                          "Choreographer",
                          "Actor",
                          "Dancer"
                      }
                  },
                  new Question()
                  {
                      Prompt = "Who is Superman?",
                      AnswerIndex = 1,
                      Options = new List<string>()
                      {
                          "Astrologer",
                          "SuperHero",
                          "Actor",
                          "Dancer"
                      }
                  },
                  new Question()
                  {
                      Prompt = "Who destroyed half the universe recently?",
                      AnswerIndex = 1,
                      Options = new List<string>()
                      {
                          "IronMan",
                          "Thor",
                          "Thanos",
                          "Corona Virus"
                      }
                  },

               }
            };

            return questions;
        }
    }
}
