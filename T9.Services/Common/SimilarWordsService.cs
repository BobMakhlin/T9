using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T9.DAL.DbModels;
using System.Data.Entity;

namespace T9.Services.Common
{
    public class SimilarWordsService
    {
        #region Private Definitions
        T9_VocabularyContext m_context;
        #endregion

        public SimilarWordsService()
        {
            m_context = new T9_VocabularyContext();
        }

        public List<string> GetSimilarWords(string word)
        {
            var similarWords =
                (
                    from w in m_context.Words
                    where w.WordName.Contains(word)
                    orderby w.UsesCount descending
                    select w.WordName
                )
                .Take(3);                

            return similarWords.ToList();
        }
        public async Task<List<string>> GetSimilarWordsAsync(string word)
        {
            return await Task.Run(() => GetSimilarWords(word));
        }
    }
}
