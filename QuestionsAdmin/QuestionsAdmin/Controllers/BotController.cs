using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestionsAdmin.Models;
using QuestionsAdmin.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [HttpGet]
        public Questions Get()
        {
            return this.botRepo.GetQuestions();
        }

        
        // PUT api/<BotController>
        [HttpPut()]
        public void Put([FromBody] Questions value)
        {
            botRepo.SetQuestions(value);
        }
    }
}
