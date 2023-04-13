using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class Course
    {
     
        public string CourseCode { get; set; }
        public int DocID { get; set; }
        public string CategoryCode { get; set; }
        public string CourseName { get; set; }
        public string CourseBanner { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public decimal CourseFee { get; set; }
        public bool? IsFree { get; set; }
        public string SkillTags { get; set; }
        public int Lectures { get; set; }
        public int DurationWeek { get; set; }
    }
}
