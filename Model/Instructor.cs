using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Instructor
    {
       public int InstructorId { get; set; }    
        public string Name { get; set; }    
        public string Qualification { get; set; }
        public string EmailID { get;set; }
        public string Contact { get; set; }
        public string Skills { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Experience { get; set; }

    }
}
