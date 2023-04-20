using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public  interface IDashBoard
    {

        //Task<IEnumerable<dynamic>> GetStudentCount();
        //Task<IEnumerable<dynamic>> GetInstructorCount();

        //Task<IEnumerable<dynamic>> GetAdminCount();
        //Task<IEnumerable<dynamic>> GetCourseCount();
        Task<int> GetStudentCount();
        Task<int> GetInstructorCount();
        Task<int> GetAdminCount();
        Task<int> GetCourseCount();

        Task<IEnumerable<dynamic>> GetCountOfStudentInCourse(int year);
    }
}
