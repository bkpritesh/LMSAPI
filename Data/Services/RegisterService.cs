using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IDbConnection _dbConnection;

        public RegisterService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }



        public async Task<Account> AddAccount(Account Rs)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@FirstName", Rs.FirstName);
            parameter.Add("@LastName", Rs.LastName);
            parameter.Add("@Address", Rs.Address);
            parameter.Add("@Skills", Rs.Skills);
           
            parameter.Add("@Email", Rs.Email);

            parameter.Add("@PasswordHash", Rs.PasswordHash);

            parameter.Add("@AccountType", Rs.AccountType);
            parameter.Add("@DisplayName", Rs.DisplayName);
            parameter.Add("@VerificationToken", Rs.VerificationToken);
            // parameter.Add("@isVerified", Rs.IsVerified);


            var results = await _dbConnection.QueryAsync<Account>("AddAccount", parameter, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();

        }
    }
}
