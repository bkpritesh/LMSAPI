using Dapper;
using Data.Repositary;
using Data.Repositary.Admin;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.AdminService
{
    public  class AdminUdService : IAdminUD
    {

        private readonly IAccountID _accountID;
        private readonly IDbConnection _dbConnection;

        public AdminUdService(IConfiguration configuration, IAccountID accountID)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
            _accountID = accountID;
        }

 


        public async Task<UserDetails> AddAdminInUD(UserDetails RgDetail)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@AccountId", _accountID.AccountId);

            var InstructorID = await _dbConnection.ExecuteScalarAsync<string>("SELECT TOP 1 [InstructorCode] FROM [TBLUserDetail] ORDER BY [InstructorCode] DESC");

            int NewInstructorID;
            if (!string.IsNullOrEmpty(InstructorID))
            {
                if (!int.TryParse(InstructorID.Substring(5), out NewInstructorID))
                {
                    NewInstructorID = 0;
                }
            }
            else
            {
                NewInstructorID = 0;
            }
            var nextInstructorID = $"I-{NewInstructorID + 1:0000}";
            _accountID.InstructorId = nextInstructorID;


            parameters.Add("@IsInstructor", RgDetail.IsInstructor);
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
            parameters.Add("@IsLeaving ", RgDetail.IsLeaving);

            //

            parameters.Add("@ProfileDes", RgDetail.ProfileDes);
            //
            parameters.Add("@ProfileImg", RgDetail.ProfileImg);

            parameters.Add("@AccountType", RgDetail.AccountType);

            parameters.Add("@InstructorCode", _accountID.InstructorId);


            var results = await _dbConnection.QueryAsync<UserDetails>("AddAdmin", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }
    }
}
