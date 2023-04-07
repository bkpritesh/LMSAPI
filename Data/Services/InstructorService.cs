using Dapper;
using Data.Repositary;
using Microsoft.Extensions.Configuration;
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


        private readonly IDbConnection _dbConnection;

        public InstructorService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<dynamic>> GetInstructor()
        {
            var results = await _dbConnection.QueryAsync("SELECT [UGUID], [Email] ,[FName] ,[MName] ,[LName] ,[Address] ,[State] ,[City] ,[Country] ,[ContactNo] ,[Education] ,[SkillSet] ,[BirthDate] ,[JoiningDate] ,[ProfileImg] ,[StudentCode] FROM [LMS].[dbo].[TBLUserDetail] where IsInstructor='true'");
            return results;
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
