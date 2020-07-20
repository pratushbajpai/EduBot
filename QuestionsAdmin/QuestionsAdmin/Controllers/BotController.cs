using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestionsAdmin.Models;
using QuestionsAdmin.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.AccessControl;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuestionsAdmin.Controllers
{
    [Route("api/[controller]")]
    public class BotController : Controller
    {
        BotRepository botRepo;
        public BotController(BotRepository botRepo)
        {
            this.botRepo = botRepo;
        }

        // GET: api/<BotController>
        [Route("[action]/{chapter}")]
        [HttpGet]
        public Questions GetByChapter(int chapter)
        {
            if(chapter == 0)
            {
                return this.botRepo.GetQuestions();
            }

            List < Question > questionList = this.botRepo.GetQuestions().QuestionsList.Where(a => a.Chapter == chapter).ToList();
            var questions = new Questions();
            questions.QuestionsList = questionList;
            return questions;
        }

        [Route("[action]")]
        [HttpGet]
        public List<string> AvailableChapters()
        {
            var availableChapters = this.botRepo.GetQuestions().QuestionsList.Select(x => "Chapter " + x.Chapter).Distinct().ToList();
            availableChapters.Insert(0, "All chapters");
            return availableChapters;
        }

        // PUT api/<BotController>
        [HttpPut()]
        public void Put([FromBody] Questions value)
        {
            botRepo.SetQuestions(value);
        }
    }
}
