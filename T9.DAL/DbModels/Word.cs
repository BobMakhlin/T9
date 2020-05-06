using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T9.DAL.DbModels
{
    public class Word
    {
        public int WordId { get; set; }
        public string WordName { get; set; }
        public int UsesCount { get; set; }
    }
}
