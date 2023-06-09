﻿using Dapper;
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
using System.Reflection.Metadata;
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


        public async Task<UserDetails> AddUserDetail(UserDetails RgDetail)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@AccountId", _accountId.AccountId);

            var  StudID  = await _dbConnection.ExecuteScalarAsync<string>("SELECT TOP 1 StudentCOde FROM [TBLUserDetail] ORDER BY StudentCOde DESC");

            int newStudId = 0;
            if (!string.IsNullOrEmpty(StudID))
            {
                if (!int.TryParse(StudID.Substring(2), out newStudId))
                {
                    newStudId = 0;
                }
            }
            var nextStudentID = $"S-{newStudId + 1:D4}";
    
            _accountId.StudentID = nextStudentID;



            parameters.Add("@IsStudent", RgDetail.IsStudent);
            parameters.Add("@Email", RgDetail.Email);
            parameters.Add(@"Fname" ,RgDetail.FName);
            parameters.Add("@Mname", RgDetail.MName);
            parameters.Add("@Lname" ,RgDetail.LName);
            parameters.Add("@Address",RgDetail.Address);
            parameters.Add("@State", RgDetail.State);
            parameters.Add("@City",   RgDetail.City);
            parameters.Add("@Country",RgDetail.Country);
            parameters.Add("@ContactNo",RgDetail.ContactNo);
            parameters.Add("@Education",RgDetail.Education);
            parameters.Add("@SkillSet",RgDetail.SkillSet);
            parameters.Add("@BirthDate",RgDetail.BirthDate);
            parameters.Add("@JoiningDate",RgDetail.JoiningDate);
            parameters.Add("@LeavingDate", RgDetail.LeavingDate);
            parameters.Add("@IsLeaving ", RgDetail.IsLeaving);

            
            //
            
            parameters.Add("@ProfileDes", RgDetail.ProfileDes);
            //
            parameters.Add("@ProfileImg", RgDetail.ProfileImg);
            
            parameters.Add("@AccountType",RgDetail.AccountType);

            parameters.Add("@StudentCode", _accountId.StudentID);


            var results = await _dbConnection.QueryAsync<UserDetails>("InsertUserDetail", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }





        public async Task<UserDetails> UpdateStudetsDetails(UserDetails RgDetail)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@AccountId", RgDetail.AccountId);
            parameters.Add("@Email", RgDetail.Email);
            parameters.Add(@"Fname", RgDetail.FName);
            parameters.Add("@Mname", RgDetail.MName);
            parameters.Add("@Lname", RgDetail.LName);
            parameters.Add("@Address", RgDetail.Address);
            parameters.Add("@State", RgDetail.State);
            parameters.Add("@City", RgDetail.City);
            parameters.Add("@Country", RgDetail.Country);
            parameters.Add("@ContactNo", RgDetail.ContactNo);
            parameters.Add("@Education", RgDetail.Education);
            parameters.Add("@SkillSet", RgDetail.SkillSet);
            parameters.Add("@BirthDate", RgDetail.BirthDate);
            parameters.Add("@JoiningDate", RgDetail.JoiningDate);
            parameters.Add("@LeavingDate", RgDetail.LeavingDate);
            parameters.Add("@IsLeaving ", RgDetail.IsLeaving);

            //

            parameters.Add("@ProfileDes", RgDetail.ProfileDes);
            //
            parameters.Add("@ProfileImg", RgDetail.ProfileImg);

    


            var results = await _dbConnection.QueryAsync<UserDetails>("UpdateStudentUserDetails", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }




        public async Task<UserDetails> GetStudentDetailsByID(string StudentCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StudentCode", StudentCode);
            var results = await _dbConnection.QueryAsync<UserDetails>("GetStudentDetailsByID", parameters, commandType: CommandType.StoredProcedure);
            return results.FirstOrDefault();
        }



        public async Task<UserDetails> GetUserDetailsByID(string AccountID)
        {
            var results = await _dbConnection.QueryAsync<UserDetails>("SELECT * FROM TBLUserDetail WHERE AccountId = @accountId", new { accountId = AccountID });
            return results.FirstOrDefault();
        }

    }
}