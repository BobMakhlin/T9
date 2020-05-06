using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T9.DAL.DbModels;

namespace T9.DAL.DbInitializers
{
    class MyDbInitializer : CreateDatabaseIfNotExists<T9_VocabularyContext>
    {
        protected override void Seed(T9_VocabularyContext context)
        {
            // English words.
            AddWordsFromJsonFile("eng_words.json", context);

            // Russian words.
            AddWordsFromJsonFile("rus_words.json", context);
        }

        void AddWordsFromJsonFile(string filename, T9_VocabularyContext context)
        {
            string json = File.ReadAllText(filename);
            string[] words = JsonConvert.DeserializeObject<string[]>(json);

            context.Words.AddRange
            (
                words.Select(word => new Word
                {
                    WordName = word
                })
            );
        }
    }
}
