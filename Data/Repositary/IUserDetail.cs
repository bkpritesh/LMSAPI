using Model;
using Model.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public interface IUserDetail
    {
        Task<IEnumerable<UserDetails>> GetUserDetail();

        // Task<UserDetails> AddUserDetail(RequestRegister RgDetail);
        Task<UserDetails> AddUserDetail(UserDetails RgDetail);

        Task<UserDetails> UpdateStudetsDetails(UserDetails RgDetail);

        Task<dynamic> GetStudentDetailsByID(string StudentCode);
    }
}
