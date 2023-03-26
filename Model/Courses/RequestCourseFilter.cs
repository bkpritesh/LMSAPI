using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Courses
{
	public class RequestCourseFilter
	{
		public string? CategoryCode { get; set; }
		public string? SearchKeyWord { get; set; }
		public bool? Price { get; set; }
		public int? Level { get; set; }
		public string? SkillTags { get; set; }
		public string? CourseKeyWord { get; set; }
	}
}
