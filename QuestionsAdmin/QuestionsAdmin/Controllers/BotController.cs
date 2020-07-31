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

        [Route("[action]/{email}/{chapter}")]
        [HttpGet]
        public Questions GetByChapterForUser(string email, int chapter)
        {
            Questions questionsForChapter;

            email = email?.ToLowerInvariant();

            if (chapter == 0)
            {
                questionsForChapter = this.botRepo.GetQuestions();
            }
            else
            {
                List<Question> questionList = this.botRepo.GetQuestions().QuestionsList.Where(a => a.Chapter == chapter).ToList();
                questionsForChapter = new Questions();
                questionsForChapter.QuestionsList = questionList;
            }

            string chapterString = "chapter " + chapter;
            QuestionTableEntity entity = this.botRepo.GetTestHistoryForUser(email, chapterString);
            if (entity == null)
            {
                questionsForChapter.LastTaken = false;
             
            }
            else 
            {
                questionsForChapter.LastTaken = true;
                questionsForChapter.LastTakenRelativeTime = BotRepository.GetPrettyDate(entity.LastTestTakenTime);
                questionsForChapter.TotalQuestions = entity.TotalQuestions;
                questionsForChapter.CorrectAnsweredQuestions = entity.CorrectAnsweredQuestions;
            }
            return questionsForChapter;
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

        [Route("[action]")]
        [HttpPut()]
        public void QuizResult([FromBody] QuizResult value)
        {
            botRepo.UpsertHistory(value);
        }

        [Route("[action]/{word}")]
        [HttpGet]
        public BingUri GetBingUri(string word)
        {
            string urlify = BotRepository.URLifyString(word);
            string uri = String.Format("https://www.bing.com/search?q={0}", urlify);
            return new BingUri(uri);
        }
    }
}
