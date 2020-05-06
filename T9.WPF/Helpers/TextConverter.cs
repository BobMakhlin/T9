using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace T9.WPF.Helpers
{
    class TextConverter
    {
        #region Private Definitions
        const string m_russianLayout = "Ё!\"№;%:?*()_+ЙЦУКЕНГШЩЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ё1234567890-=йцукенгшщзхъ\\фывапролджэячсмитьбю. ";
        const string m_englishLayout = "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./ ";
        #endregion

        public string ConvertToRussianLayout(string text)
        {
            return ConvertToLayout(text, m_englishLayout, m_russianLayout);
        }
        public string ConvertToEnglishLayout(string text)
        {
            return ConvertToLayout(text, m_russianLayout, m_englishLayout);
        }

        string ConvertToLayout(string text, string sourceLayout, string destLayout)
        {
            var sb = new StringBuilder();

            foreach (char symbol in text)
            {
                int index = sourceLayout.IndexOf(symbol);
                sb.Append(destLayout[index]);
            }

            return sb.ToString();
        }
    }
}
