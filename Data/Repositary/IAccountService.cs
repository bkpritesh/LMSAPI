using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public interface IAccountService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        //public AuthenticateResponse Authenticate(AuthenticateRequest model);
        Task<AuthenticateResponse> RefreshToken(string token);
    }
}
