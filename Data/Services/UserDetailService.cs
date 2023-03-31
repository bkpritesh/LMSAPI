using Dapper;
using Data.Repositary;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
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
    public class UserDetailService : IUserDetail
    {
        private readonly IAccountID _accountId;
        private readonly IDbConnection _dbConnection;



        public UserDetailService(IConfiguration configuration, IAccountID accountId)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
            _accountId = accountId;

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
            parameter.Add("@AccountId", _accountId.AccountId);

            //   parameter.Add("@AccountID",UserDetails.AccountId);
            parameter.Add("@IsInstructor", UserDetails.IsInstructor);
            parameter.Add("@IsStudent", UserDetails.IsStudent);
            parameter.Add("@IsAdmin", UserDetails.IsAdmin);
            parameter.Add("@IsGuest", UserDetails.IsGuest);
            parameter.Add("@Email", UserDetails.Email);
            parameter.Add(@"Fname",UserDetails.FName);
            parameter.Add("@Mname",UserDetails.MName);
            parameter.Add("@Lname",UserDetails.LName);
            parameter.Add("Address",UserDetails.Address);
            parameter.Add("Status",UserDetails.Status);
            //parameter.Add("",UserDetails.);
            //parameter.Add("",UserDetails.);
            //parameter.Add("",UserDetails.);
            //parameter.Add("",UserDetails.);
            //parameter.Add("",UserDetails.);
            //parameter.Add("",UserDetails.);
            //parameter.Add("",UserDetails.);
            //parameter.Add("",UserDetails.);
            //parameter.Add("", UserDetails.);

            var results = await _dbConnection.QueryAsync<UserDetails>("InsertUserDetail", parameter, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }



        //public async Task<UserDetails> AddStudent(UserDetails student)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@FirstName", student.FirstName);
        //    parameters.Add("@LastName", student.LastName);
        //    parameters.Add("@EmailID", student.EmailID);
        //    parameters.Add("@DateOfBirth", student.DateOfBirth);
        //    parameters.Add("@Address", student.Address);
        //    parameters.Add("@City", student.City);
        //    parameters.Add("@PinCode", student.PinCode);
        //    parameters.Add("@State", student.State);
        //    parameters.Add("@Country", student.Country);
        //    parameters.Add("@MobileNo", student.MobileNo);
        //    parameters.Add("@Qualification", student.Qualification);
        //    parameters.Add("@CourseName", student.CourseName);

        //    var results = await _dbConnection.QueryAsync<Student>("AddStudent", parameters, commandType: CommandType.StoredProcedure);
        //    return results.SingleOrDefault();
        //}
    }
}