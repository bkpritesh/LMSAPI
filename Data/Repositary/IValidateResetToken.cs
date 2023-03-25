using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositary
{
public interface  IValidateResetToken
    {
        Task<(bool isValid, bool isExpired)> ValidateResetToken(string resetToken);
    }
}
