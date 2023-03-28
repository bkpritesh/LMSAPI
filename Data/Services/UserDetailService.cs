using Dapper;
using Data.Repositary;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Model;
using Model.Students;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class UserDetailService:IUserDetail
    {
        private readonly IDbConnection _dbConnection;


        public UserDetailService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }



        public async Task<IEnumerable<UserDetails>> GetUserDetail()
        {
            Log.Error("Service");

            var sql = "SELECT * FROM TBLUserDetail";
            var results = await _dbConnection.QueryAsync<UserDetails>(sql);

           // var results = await _dbConnection.QueryAsync<RequestRegister>("GetCourse", commandType: CommandType.StoredProcedure);
            return results;
        }


        public async Task<UserDetails> AddUserDetail(UserDetails UserDetails)
        {


            var parameter = new DynamicParameters();
            parameter.Add("@AccountID",UserDetails.AccountId);
            parameter.Add("@IsInstructor", UserDetails.IsInstructor);
            parameter.Add("@IsStudent", UserDetails.IsStudent);
            parameter.Add("@IsAdmin", UserDetails.IsAdmin);
            parameter.Add("@IsGuest", UserDetails.IsGuest);
            parameter.Add("@Email", UserDetails.Email);
           

            var results = await _dbConnection.QueryAsync<UserDetails>("InsertUserDetail", parameter, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }
    }
}
