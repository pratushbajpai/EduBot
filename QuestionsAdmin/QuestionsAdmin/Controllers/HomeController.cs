using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using QuestionsAdmin.Models;
using QuestionsAdmin.Repository;

namespace QuestionsAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        BotRepository botRepo;
        
        public Questions questions { get; set; }

        public HomeController(ILogger<HomeController> logger, BotRepository botRepo)
        {
            _logger = logger;

            this.botRepo = botRepo;
            questions = botRepo.InMemoryQuestions;
        }

        public IActionResult Index()
        {
            return View(new Questions { QuestionsList = questions.QuestionsList});
        }

        public IActionResult Chat()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Edit(String Id)
        {
            //Get the student from studentList sample collection for demo purpose.
            //Get the student from the database in the real application
            var question = questions.QuestionsList.Where(qid => qid.Id == Id).FirstOrDefault();
            return View(question);
        }

        public ActionResult Add()
        {
            return View();
        }

        public IActionResult Create(Question question)
        {
            questions.QuestionsList.Add(question);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string Id)
        {
            var savedQuestion = questions.QuestionsList.Where(qid => qid.Id == Id).FirstOrDefault();
            questions.QuestionsList.Remove(savedQuestion);
            return RedirectToAction("Index");
        }

        public IActionResult Save()
        {
            botRepo.SetQuestions(questions);

            //get fresh questions from blob
            questions = botRepo.GetQuestions();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Question question)
        {
            //write code to update student 
            var savedQuestion = questions.QuestionsList.Where(qid => qid.Id == question.Id).FirstOrDefault();
            savedQuestion.Prompt = question.Prompt;
            savedQuestion.AnswerIndex = question.AnswerIndex;
            savedQuestion.OptionValues = question.OptionValues;
            savedQuestion.Chapter = question.Chapter;
            //savedQuestion.Options = question.Options;

            return RedirectToAction("Index");  
        }
    }
}
