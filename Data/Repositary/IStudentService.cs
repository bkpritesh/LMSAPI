using Model;
using Model.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public interface IStudentService
    {



        Task<IEnumerable<StudentFNameandLnameBinding>> GetStudent();
        Task<GetStudentEnrolledInCourse> GetEnrolledCourseByStudentID(string studentEnrollment);
        //Task<Student> GetStudentByID(int id);
        //Task<Student> AddStudent(Student student);

        //Task<Student> UpdateStudent(Student student);

        //Task<IEnumerable<Student>> DeleteStudent(int id);

    }
}
