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

        Task<UserDetails> AddInstructorDetail(UserDetails RgDetail);

        Task<IEnumerable<dynamic>> GetBDByInstructorCode(string InstructorCode);
  
    }
}
