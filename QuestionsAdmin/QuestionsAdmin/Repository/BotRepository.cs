using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using QuestionsAdmin.Models;
using Newtonsoft.Json;

namespace QuestionsAdmin.Repository
{
    public class BotRepository
    {
        public string questionBlobContainer;
        public string questionsBlob;

        BlobHttpHeaders properties = new BlobHttpHeaders()
        {
            ContentType = "application-json",
        };

        public string blobConnectionString;

        BlobContainerClient blobContainerClient;
        public Questions InMemoryQuestions;

        public BotRepository(string blobConnectionString)
        {
            this.blobConnectionString = blobConnectionString;
            questionBlobContainer = "botconfig";
            questionsBlob = "questions";
            InMemoryQuestions = GetQuestions();
        }

        public Questions GetQuestions()
        {
            BlobClient blobClient;
            
            blobContainerClient = new BlobContainerClient(blobConnectionString, questionBlobContainer);
            blobClient = blobContainerClient.GetBlobClient(questionsBlob);
            if(blobClient.Exists())
            {
                //blob exists - download
                var downloadedBlob = blobClient.Download();

                using (MemoryStream ms = new MemoryStream())
                {
                    downloadedBlob.Value.Content.CopyTo(ms);
                    var content = Encoding.UTF8.GetString(ms.ToArray());
                    InMemoryQuestions = JsonConvert.DeserializeObject<Questions>(content);
                }
            }
            else
            {
                InMemoryQuestions = GetInitialQuestions();
                var questionContent = JsonConvert.SerializeObject(InMemoryQuestions);
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(questionContent)))
                {
                    blobClient.Upload(ms, properties);
                }
            }    
            return InMemoryQuestions;
        }

        public void SetQuestions(Questions question)
        {
            blobContainerClient = new BlobContainerClient(blobConnectionString, questionBlobContainer);
            var blobClient = blobContainerClient.GetBlobClient(questionsBlob);
            if (blobClient.Exists())
            {
                blobClient.SetHttpHeaders(properties);
                var questionContent = JsonConvert.SerializeObject(InMemoryQuestions);
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(questionContent)))
                {
                    blobClient.Upload(ms, properties);
                }
            }
            else
            {
                var questionContent = JsonConvert.SerializeObject(InMemoryQuestions);
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(questionContent)))
                {
                    blobClient.Upload(ms, properties);
                }
            }

            //Fetch questions again
            InMemoryQuestions = GetQuestions();
        }
         
        public Questions GetInitialQuestions()
        {
            Questions questions = new Questions()
            {
                QuestionsList = new List<Question>()
               {
                  new Question()
                  {
                      Id = "id1",
                      Prompt = "Who was Gallelio?",
                      AnswerIndex = 0,
                      OptionValues = "Astrologer|Choreographer|Actor|Dancer"
                      //Options = new List<string>()
                      //{
                      //    "Astrologer",
                      //    "Choreographer",
                      //    "Actor",
                      //    "Dancer"
                      //}
                  },
                  new Question()
                  {
                      Id = "id2",
                      Prompt = "Who is Superman?",
                      AnswerIndex = 1,
                      OptionValues = "Astrologer|SuperHero|Actor|Dancer"
                      //Options = new List<string>()
                      //{
                      //    "Astrologer",
                      //    "SuperHero",
                      //    "Actor",
                      //    "Dancer"
                      //}
                  },
                  new Question()
                  {
                      Id = "id3",
                      Prompt = "Who destroyed half the universe recently?",
                      AnswerIndex = 2,
                      OptionValues = "IronMan|Thor|Thanos|Corona Virus"
                      //Options = new List<string>()
                      //{
                      //    "IronMan",
                      //    "Thor",
                      //    "Thanos",
                      //    "Corona Virus"
                      //}
                  },

               }
            };

            return questions;
        }

    }
}
