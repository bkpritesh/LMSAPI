using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class CourseSyllabus
	{
		public IEnumerable<BatchChapter> chapters { get; set; }
		public Course Course { get; set; }

	}
}
