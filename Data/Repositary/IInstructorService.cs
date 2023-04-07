using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public interface IInstructorService
    {

        Task<IEnumerable<dynamic>> GetInstructor();
        //    Task<IEnumerable<Instructor>> GetInstructors();
        //Task<Instructor> GetInstructorByID(int id);
        //Task<Instructor> AddInstructor(Instructor instructor);
        //Task<Instructor> UpdateInstructor(Instructor instructor);
        //Task<IEnumerable<Instructor>> DeleteInstructorById(int id);

    }
}
