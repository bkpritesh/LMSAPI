using Data.Repositary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDbConnection _dbConnection;

        public AccountService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            AuthenticateResponse authenticate= new AuthenticateResponse();
            //Temp condtion
            if (model.Email == "test@test.com" && model.Password == "Test@123")
            {
                Account account = new Account();
                string token = generateJwtToken(account);
                authenticate.RefreshToken = GenerateRefreshToken().Token;
                authenticate.RefreshToken = "CCD1F3DB9AA432B6358BE1D40BCAC04FD3D32BFBC48ADAAB66E656B9A84A79F6E76CC4860A9BE220FAE76B49E56525F4C42A189D0E240E3F96F56F679F930FB9";
                authenticate.AccountType = "Admin";
                authenticate.AccountId = Guid.NewGuid().ToString();
                authenticate.FirstName = "Pritesh";
                authenticate.LastName = "Maturkar";
                authenticate.JwtToken= token;

            }
            return authenticate;
        }
        private string generateJwtToken(Account account)
        {  
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, "Admin"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("AccountId",  Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GyanShaktiJWTKey"));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDes = new JwtSecurityToken(
                "GyanShaktiTech",
                "GyanShaktiTech.com",
                claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: signIn);

            var token = tokenHandler.WriteToken(tokenDes);
            return token;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                // token is a cryptographically strong random sequence of values
                Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                // token is valid for 7 days
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = "GyanShati"
            };

            // ensure token is unique by checking against db
            var tokenIsUnique = true;

            if (!tokenIsUnique)
                return GenerateRefreshToken();

            return refreshToken;
        }

        public AuthenticateResponse RefreshToken(string token)
        {
            AuthenticateResponse authenticate = new AuthenticateResponse();
            
            
           
            // Cheeck token in DB
            if (token == "CCD1F3DB9AA432B6358BE1D40BCAC04FD3D32BFBC48ADAAB66E656B9A84A79F6E76CC4860A9BE220FAE76B49E56525F4C42A189D0E240E3F96F56F679F930FB9")
            {
                Account account = new Account();
                authenticate.RefreshToken = GenerateRefreshToken().Token;
                authenticate.RefreshToken = "CCD1F3DB9AA432B6358BE1D40BCAC04FD3D32BFBC48ADAAB66E656B9A84A79F6E76CC4860A9BE220FAE76B49E56525F4C42A189D0E240E3F96F56F679F930FB9";
                authenticate.AccountType = "Admin";
                authenticate.AccountId = Guid.NewGuid().ToString();
                authenticate.FirstName = "Pritesh";
                authenticate.LastName = "Maturkar";
                authenticate.JwtToken = generateJwtToken(account);

            }
            // replace old refresh token with a new one(rotate token)
            // remove old refresh tokens from account
            // save changes to db
            return authenticate;
        }
    }

    
}
