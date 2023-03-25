using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model
{
    public class AuthenticateResponse
    {
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AccountType { get; set; }
        public string JwtToken { get; set; }

       // [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
    }
}
