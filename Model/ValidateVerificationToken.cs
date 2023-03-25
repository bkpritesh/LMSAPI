using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ValidateVerificationToken
    {

        public string VerificationToken { get; set; }
        public Boolean IsVerified { get; set; }
        public DateTime VerifiedOn { get; set; }
    }
}
