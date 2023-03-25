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
        public AuthenticateResponse Authenticate(AuthenticateRequest model);
        public AuthenticateResponse RefreshToken(string token);
    }
}
