using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Account
    {
        //public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Skills { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }   
        public string AccountType { get; set; }
        public string DisplayName { get; set; }
        //public string ResetToken { get; set; }  
      public string VerificationToken { get; set; }
       //  public Boolean IsVerified { get; set; }
        //public DateTime VerifiedOn { get; set; }

    }
}
