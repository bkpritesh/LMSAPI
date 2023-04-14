﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class Batch
    {
        public string BatchCode { get; set; }

        public string? BatchName { get; set; }

        public string? CourseCode { get; set; }

        

        public int Assessment { get; set; }
        public string? Description { get; set; }
        public string Students { get; set; }
        public string StartTime { get; set;}
        public string EndTime { get; set; }
        public string  InstructorCode { get; set;}

    }
}
