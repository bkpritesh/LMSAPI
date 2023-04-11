using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BindingAccountService


  
    {
        public string RefreshToken { get; set; }
        public string AccountType { get; set; }
        public string Email { get; set; }   
        public string PasswordHash { get; set; }    
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string JwtToken { get; set; }
    }


}
