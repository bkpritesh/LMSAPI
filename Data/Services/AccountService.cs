using Dapper;
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
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            AuthenticateResponse authenticate= new AuthenticateResponse();
                string email = await _dbConnection.QueryFirstOrDefaultAsync<string>(
                "SELECT Email FROM TBLAccount WHERE Email = @Email",
                new { model.Email });

            if (email != null)
            {

                BindingAccountService binding = await _dbConnection.QueryFirstOrDefaultAsync<BindingAccountService>(
                "SELECT AccountId, PasswordHash, AccountType, FirstName, LastName,Email  FROM TBLAccount WHERE Email = @Email",
                new { Email = model.Email });


                ////var results = await _dbConnection.QueryAsync("GetCitiesByStateId", new { stateId }, commandType: CommandType.StoredProcedure);
                //if (binding == null)
                //{

                //    throw new Exception();
                //}



                if (binding != null && model.Email.ToLower() == email.ToLower() && model.Password == binding.PasswordHash)
                {




                    Account account = new Account();
                    string token = generateJwtToken(account);
                    authenticate.RefreshToken = GenerateRefreshToken().Token;
                    //get the refresh token from the database 
                    authenticate.AccountId = binding.AccountId;
                    var RFTOKEN = await _dbConnection.ExecuteAsync(
                            "UPDATE TBLAccount SET RefreshToken = @RefreshToken WHERE AccountId = @AccountId",
                            new { authenticate.RefreshToken, authenticate.AccountId }
                        );
                    authenticate.RefreshToken = RFTOKEN.ToString();
                    authenticate.AccountType = binding.AccountType;

                    authenticate.Email = binding.Email;
                    authenticate.FirstName = binding.FirstName;
                    authenticate.LastName = binding.LastName;
                    authenticate.JwtToken = token;

                }
            }
            return authenticate;
        }
        private string generateJwtToken(Account account)
        {  
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>() {
                // get the Username 
                new Claim(ClaimTypes.Name, "Admin"),

                // get role account type
                new Claim(ClaimTypes.Role, "Admin"),

                new Claim("AccountId",  Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GyanShaktiJWTKey"));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDes = new JwtSecurityToken(
                "GyanShaktiTech",
                "GyanShaktiTech.com",
                claims,
                expires: DateTime.UtcNow.AddDays(7),
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
            bool tokenIsUnique;

            do
            {
                // Query the database to check if the token is unique
                var existingToken = _dbConnection.QuerySingleOrDefault<RefreshToken>("SELECT RefreshToken from TBLAccount WHERE RefreshToken = @Token", new { Token = refreshToken.Token });

                tokenIsUnique = existingToken == null;

                if (!tokenIsUnique)
                {
                    return GenerateRefreshToken();
                    // If the token is not unique, generate a new one and check again
                  //  refreshToken.Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
                }

            } while (!tokenIsUnique);

            return refreshToken;
            //var existingTdoken = _dbConnection.QuerySingleOrDefault<RefreshToken>("SELECT * FROM RefreshTokens WHERE Token = @Token", new { Token = refreshToken.Token });
            //tokenIsUnique = existingToken == null;
            //if (!tokenIsUnique)
            //    return GenerateRefreshToken();

            //return refreshToken;
        }

     //   public Task<AuthenticateResponse> RefreshToken(string token)
     //   {
     //       {
     //       AuthenticateResponse authenticate = new AuthenticateResponse();

     //           BindingAccountService binding = await _dbConnection.QueryFirstOrDefaultAsync<BindingAccountService>(
     //"SELECT AccountId, PasswordHash, AccountType, FirstName, LastName,Email  FROM TBLAccount WHERE Email = @Email",
     //new { Email = model.Email }

     //       // Cheeck token in DB
     //       if (token == "CCD1F3DB9AA432B6358BE1D40BCAC04FD3D32BFBC48ADAAB66E656B9A84A79F6E76CC4860A9BE220FAE76B49E56525F4C42A189D0E240E3F96F56F679F930FB9")
     //       {
     //           Account account = new Account();
     //           authenticate.RefreshToken = GenerateRefreshToken().Token;
     //           authenticate.RefreshToken = "CCD1F3DB9AA432B6358BE1D40BCAC04FD3D32BFBC48ADAAB66E656B9A84A79F6E76CC4860A9BE220FAE76B49E56525F4C42A189D0E240E3F96F56F679F930FB9";
     //           authenticate.AccountType = "Admin";
     //           authenticate.AccountId = Guid.NewGuid().ToString();
     //           authenticate.FirstName = "Pritesh";
     //           authenticate.LastName = "Maturkar";
     //           authenticate.JwtToken = generateJwtToken(account);

     //       }
     //       // replace old refresh token with a new one(rotate token)
     //       // remove old refresh tokens from account
     //       // save changes to db
     //       return authenticate;
     //   }


            public async Task<AuthenticateResponse> RefreshToken(string token)
            {
                AuthenticateResponse authenticate = new AuthenticateResponse();

                // Check token in DB
                BindingAccountService binding = await _dbConnection.QueryFirstOrDefaultAsync<BindingAccountService>(
                    "SELECT AccountId, AccountType, FirstName, LastName , RefreshToken From TBLAccount WHERE [RefreshToken] = @token",
                    new { token }
                );

                if (binding != null && token == binding.RefreshToken)
                {
                    Account account = new Account();
                    authenticate.RefreshToken = GenerateRefreshToken().Token;
                    authenticate.AccountType = binding.AccountType;
                    authenticate.AccountId = binding.AccountId;
                    authenticate.FirstName = binding.FirstName;
                    authenticate.LastName = binding.LastName;
                    authenticate.JwtToken = generateJwtToken(account);

                    // Replace old refresh token with a new one (rotate token)
                    binding.RefreshToken = authenticate.RefreshToken;
                
                    // Save changes to DB
                    await _dbConnection.ExecuteAsync(
                        "UPDATE TBLAccount SET RefreshToken = @RefreshToken WHERE AccountId = @AccountId",
                        new { binding.RefreshToken, binding.AccountId }
                    );
                }

                // Return the authenticate object wrapped in a completed Task
                return authenticate;
            }

        }


    }
