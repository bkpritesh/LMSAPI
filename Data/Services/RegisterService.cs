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
        private readonly IAccountID _accountID;
        private readonly IDbConnection _dbConnection;

        public RegisterService(IConfiguration configuration, IAccountID accountID)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
            _accountID = accountID;
        }



        //public async Task<string> AddAccount(Account Rs)
        //{

        //    var parameter = new DynamicParameters();
        //    Guid accountId = Guid.NewGuid();
        //    parameter.Add("@AccountId", accountId);
        //    parameter.Add("@FirstName", Rs.FirstName);
        //    parameter.Add("@LastName", Rs.LastName);
        //    parameter.Add("@Address", Rs.Address);
        //    parameter.Add("@Skills", Rs.Skills);

        //    parameter.Add("@Email", Rs.Email);

        //    parameter.Add("@PasswordHash", Rs.PasswordHash);

        //    parameter.Add("@Acc`ountType", Rs.AccountType);
        //    parameter.Add("@DisplayName", Rs.DisplayName);
        //    parameter.Add("@VerificationToken", Rs.VerificationToken);
        //    // parameter.Add("@isVerified", Rs.IsVerified);


        //    await _dbConnection.QueryAsync<Account>("AddAccount", parameter, commandType: CommandType.StoredProcedure);

        //    return accountId.ToString();


        //}


        public async Task<Account> AddAccount(Account Rs)
        {

            var parameter = new DynamicParameters();


            parameter.Add("@AccountId", _accountID.AccountId);
            parameter.Add("@FirstName", Rs.FirstName);
            parameter.Add("@LastName", Rs.LastName);
            parameter.Add("@Address", Rs.Address);
            parameter.Add("@Skills", Rs.Skills);
            parameter.Add("@Email", Rs.Email);
            parameter.Add("@PasswordHash", Rs.PasswordHash);
            parameter.Add("@AccountType", Rs.AccountType);
            parameter.Add("@DisplayName", Rs.DisplayName);
            parameter.Add("@VerificationToken", Rs.VerificationToken);

            var results = await _dbConnection.QueryAsync<Account>("AddAccount", parameter, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        //    return newAccountId;

            // Create a new UserDetails object with the generated accountId and return it
            //UserDetails userDetails = new UserDetails
            //{
            //    AccountId = accountId.ToString(),
            //    Email = Rs.Email,
            //    FName = Rs.FirstName,
              
            //    LName = Rs.LastName,
            //    Address = Rs.Address,
             
            //    SkillSet = Rs.Skills,
            //    BirthDate = DateTime.MinValue,
            //    JoingDate = DateTime.Now,

            //    AccounType = Rs.AccountType,
         
            //};

           
        }

    }
}
