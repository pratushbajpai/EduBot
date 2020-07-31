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
using Microsoft.Azure.Cosmos.Table;
using QuestionsAdmin.Models;
using Newtonsoft.Json;

namespace QuestionsAdmin.Repository
{
    public class BotRepository
    {
        public string questionBlobContainer;
        public string questionsBlob;
        public string questionStateTable;
        public CloudTableClient tableClient;


        BlobHttpHeaders properties = new BlobHttpHeaders()
        {
            ContentType = "application-json",
        };

        public string connectionString;

        BlobContainerClient blobContainerClient;
        public Questions InMemoryQuestions;

        public BotRepository(string connectionString)
        {
            this.connectionString = connectionString;
            questionBlobContainer = "botconfig";
            questionStateTable = "quizhistory";
            questionsBlob = "questions";
            InMemoryQuestions = GetQuestions();
            var storageAccount = CloudStorageAccount.Parse(this.connectionString);
            tableClient = storageAccount.CreateCloudTableClient();
        }

        public Questions GetQuestions()
        {
            if(this.connectionString == "test")
            {
                return GetInitialQuestions();
            }

            BlobClient blobClient;
            blobContainerClient = new BlobContainerClient(this.connectionString, questionBlobContainer);
            blobClient = blobContainerClient.GetBlobClient(questionsBlob);
            if (blobClient.Exists())
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
            blobContainerClient = new BlobContainerClient(this.connectionString, questionBlobContainer);
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
            var questions  = JsonConvert.DeserializeObject<Questions>(File.ReadAllText(QuestionsFile));

            /*            Questions questions = new Questions()
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
                        };*/

            return questions;
        }

        public QuestionTableEntity GetTestHistoryForUser(string email, string chapter)
        {
            try
            {
                return GetItem<QuestionTableEntity>(email, chapter).Result;
            }catch (Exception e)
            {
                return null;
            }
        }

        public void UpsertHistory(QuizResult result)
        {
            QuestionTableEntity entity = new QuestionTableEntity();
            entity.PartitionKey = result.Email?.ToLowerInvariant();
            string chapterString = "chapter " + result.Chapter;
            entity.RowKey = chapterString;
            entity.LastTestTakenTime = DateTime.Now;
            entity.TotalQuestions = Int32.Parse(result.TotalQuestions);
            entity.CorrectAnsweredQuestions = Int32.Parse(result.CorrectAnsweredQuestions);

            try
            {
                UpsertEntity<QuestionTableEntity>(entity, questionStateTable).ConfigureAwait(false);
            }
            catch (Exception e)
            {

            }
        }

        private async Task<T> GetItem<T>(string partititonKey, string rowKey) where T : class, ITableEntity, new()
        {
                var table = tableClient.GetTableReference(questionStateTable);
                var operation = TableOperation.Retrieve<QuestionTableEntity>(partititonKey, rowKey);
                var result = await table.ExecuteAsync(operation).ConfigureAwait(false);
                return result.Result as T;
        }

        private async Task UpsertEntity<T>(T entity, string tableName) where T : class, ITableEntity, new()
        {
            var table = tableClient.GetTableReference(tableName);

            var operation = TableOperation.InsertOrReplace(entity);

            await table.ExecuteAsync(operation).ConfigureAwait(false);
        }

        private static string QuestionsFile
        {
            get
            {
                var assemblyDirectory = Path.GetDirectoryName(typeof(Questions).Assembly.Location);
                return Path.Combine(assemblyDirectory, "InitialQuestions", "questions");
            }
        }

        public static string GetPrettyDate(DateTime d)
        {
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return null;
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "just now";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "1 minute ago";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} minutes ago",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 hour ago";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} hours ago",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "yesterday";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} days ago",
                    dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("{0} weeks ago",
                    Math.Ceiling((double)dayDiff / 7));
            }
            return null;
        }

        public static string URLifyString(string inputString)
        {
            if (inputString != null)
            {
                //Find out the length of the given string    
                int inputStringLength = inputString.Length;
                //Convert the given string into an array of type character    
                char[] inputCharacterSet = inputString.ToCharArray();

                //Find out the count of spaces within the input string     
                int spaceInStringCount = 0;
                for (int indexCount = 0; indexCount < inputStringLength; indexCount++)
                {
                    if (inputCharacterSet[indexCount] == ' ')
                    {
                        spaceInStringCount++;
                    }
                }

                // multiply the number of spaces with 2 to find out the extra space required to replace ' ' with '%20'    
                // for example if there is one ' ' space then we need two extra places     
                // the given string to add '%20'    
                int extraSpacesRequired = (spaceInStringCount * 2);
                //add extra spaces required to shift characters and replace ' ' with '%20'    
                inputCharacterSet = inputCharacterSet.Concat(new char[extraSpacesRequired]).ToArray();
                int inputCharacterSetCurrentIndex = inputStringLength - 1 + extraSpacesRequired;

                //iterate through the given string but note that start from the end and working backwards because    
                //we have added an extra buffer at the end, which allows us to change characters without worrying about what we're overwriting.    
                for (int inputIndex = inputStringLength - 1; inputIndex >= 0; inputIndex--)
                {
                    //if ' ' space found then replace ' ' with '%20'    
                    if (inputCharacterSet[inputIndex] == ' ')
                    {
                        inputCharacterSet[inputCharacterSetCurrentIndex--] = '0';
                        inputCharacterSet[inputCharacterSetCurrentIndex--] = '2';
                        inputCharacterSet[inputCharacterSetCurrentIndex--] = '%';
                    }
                    else
                    {
                        //if not found then shift current character by 2 places    
                        inputCharacterSet[inputCharacterSetCurrentIndex--] = inputCharacterSet[inputIndex];
                    }
                }

                return new string(inputCharacterSet);
            }
            return null;
        }
    }



}
