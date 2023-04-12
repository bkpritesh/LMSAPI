using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class Inquiries
    {
    
        public string Name { get; set; }    
        public string Email { get; set; } 
        public string ContactNo { get; set; }    
        public string WhatsAppNo { get; set; }
                    
        public string Message { get; set; }
        public string Purpose { get; set; }

        public bool IsLead { get; set; }  

    }
}
