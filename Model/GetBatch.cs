using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GetBatch
    {
        public string BatchCode { get; set; }

        public string? BatchName { get; set; }

        public string? CourseCode { get; set; }


        public string? CourseName { get; set; }
        public int Assessment { get; set; }
        public string? Description { get; set; }

        public string StartTIme { get; set; }
        public string EndTIme { get; set; }
        public string InstructorCode { get; set; }


        public List<NAME> Students { get; set; }
    }

    public class  NAME
    {
        public string StudentCode { get; set; }
        public string FullName { get; set; }
    }

}

