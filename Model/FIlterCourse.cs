using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class FilterCourse
    {

        public string? CategoryCode { get; set; }
        public string? CourseName { get; set; }

        public int? Level { get; set; }

        public bool? IsFree { get; set; }
        public string? SkillTags { get; set; }

    }
}
