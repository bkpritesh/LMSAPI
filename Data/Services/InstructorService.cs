using Dapper;
using Data.Repositary;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{

    public class InstructorService : IInstructorService
    {

        private readonly IAccountID _accountID;
        private readonly IDbConnection _dbConnection;

        public InstructorService(IConfiguration configuration, IAccountID accountID)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
            _accountID = accountID; 
        }

        public async Task<IEnumerable<dynamic>> GetInstructor()
        {
            var results = await _dbConnection.QueryAsync("SELECT [UGUID], [Email] ,[FName] ,[MName] ,[LName] ,[Address] ,[State] ,[City] ,[Country] ,[ContactNo] ,[Education] ,[SkillSet] ,[BirthDate] ,[JoiningDate] ,[ProfileImg] ,[InstructorCode] FROM [dbo].[TBLUserDetail] where IsInstructor='true'");
            return results;
        }



        public async Task<UserDetails> AddInstructorDetail(UserDetails RgDetail)
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
            var nextInstructorID = $"I-{NewInstructorID + 1:D4}";
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


            var results = await _dbConnection.QueryAsync<UserDetails>("InsertInstructor", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }








































        //public async Task<Instructor> GetInstructorByID(int id)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@InstructorID", id);

        //    var results = await _dbConnection.QueryAsync<Instructor>("GetInstructor", parameters, commandType: CommandType.StoredProcedure);
        //    return results.FirstOrDefault();
        //}


        //public async Task<Instructor> AddInstructor(Instructor instructor)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@Name", instructor.Name);
        //    parameters.Add("@Qualification", instructor.Qualification);
        //    parameters.Add("@EmailID", instructor.EmailID);
        //    parameters.Add("@Contact", instructor.Contact);
        //    parameters.Add("@Skills",instructor.Skills);
        //    parameters.Add("@Address", instructor.Address);
        //    parameters.Add("@State", instructor.State);
        //    parameters.Add("@City", instructor.City);
        //    parameters.Add("@Country", instructor.Country);
        //    parameters.Add("@Experience", instructor.Experience);



        //    var results = await _dbConnection.QueryAsync<Instructor>("AddInstructor", parameters, commandType: CommandType.StoredProcedure);
        //    return results.SingleOrDefault();
        //}


        //public async Task<Instructor> UpdateInstructor(Instructor instructor)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@InstructorId", instructor.InstructorId);
        //    parameters.Add("@Name", instructor.Name);
        //    parameters.Add("@Qualification", instructor.Qualification);
        //    parameters.Add("@EmailID", instructor.EmailID);
        //    parameters.Add("@Contact", instructor.Contact);
        //    parameters.Add("@Skills", instructor.Skills);
        //    parameters.Add("@Address", instructor.Address);
        //    parameters.Add("@State", instructor.State);
        //    parameters.Add("@City", instructor.City);
        //    parameters.Add("@Country", instructor.Country);
        //    parameters.Add("@Experience", instructor.Experience);



        //    var results = await _dbConnection.QueryAsync<Instructor>("UpdateInstructor", parameters, commandType: CommandType.StoredProcedure);
        //    return results.SingleOrDefault();
        //}


        //public async Task<IEnumerable<Instructor>> DeleteInstructorById(int id)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@InstructorId", id);

        //    var results = await _dbConnection.QueryAsync<Instructor>("DeleteStudentByID", parameters, commandType: CommandType.StoredProcedure);
        //    return results;
        //}
    }
}
