using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model.Courses
{
    public class GetStudentEnrolledInCourse
    {
        public string? StudentCode { get; set; }

        public string? CourseCode { get; set; }  
        public string? CourseName { get; set; }

        public string? Description { get; set; }


        public string?Lectures { get; set; }
        public string? Level { get; set; }

        public string?CourseBanner { get; set; }
     
    }
}
