using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public interface ICourse
    {

        Task<IEnumerable<Course>> GetCourse();
        Task<Course> GetCourseByID(string CourseCode);
        //  Task<Course> GetCourseByID(string CourseCode);
 

        Task<Course> UpdateCourse(Course course);
        Task<AddCourse> AddCourse(AddCourse course);
        Task<IEnumerable<Course>> DeleteCourse(string CourseCode);
    }
}
