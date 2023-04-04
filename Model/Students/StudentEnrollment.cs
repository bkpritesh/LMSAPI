using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Students
{
    public  class StudentEnrollment
    {
        public string CategoryCode { get; set; }
        public string CourseCode { get; set; }
        public decimal CourseFees { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalFees { get; set; }
 
    }
}
