using Data.Services;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
    public interface IForgotPasswordService
    {
        Task<Account> IsEmailExists(string email, string resetToken);



    }
}
