using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public interface IFilterCourse
    {
       Task<List<FilterCourse>> FilterCourses(FilterCourse Fcourse);
       Task<IEnumerable<BatchChapter>> GetCourseDetails(string CourseCode);

	}
}
