using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class ChatperBinding
    {
        public string ChapterCode { get; set; }

        public string ChapterName { get; set; }
        public string? ChapterDescription{ get; set; }
        public DateTime? ExpectedDate { get; set; }
         
    }
}
