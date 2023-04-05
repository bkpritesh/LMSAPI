using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model;
using Model.Students;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
   public class StudentEnrollmentService : IStudentEnrollment
    {
        private readonly IAccountID _accountID;
        private readonly IDbConnection _dbConnection;

        public StudentEnrollmentService(IConfiguration configuration, IAccountID accountID)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
            _accountID = accountID;
        }



        public async Task<StudentEnrollment> Enrollment(StudentEnrollment StudEnrol)
        {

            var parameter = new DynamicParameters();


            parameter.Add("@AccountId", _accountID.AccountId);
            parameter.Add("@CategoryCode", StudEnrol.CategoryCode);
            parameter.Add("@CourseCode", StudEnrol.CourseCode);
            parameter.Add("@CourseFees", StudEnrol.CourseFees);
            parameter.Add("@Discount", StudEnrol.Discount);
            parameter.Add("@TotalFees", StudEnrol.TotalFees);
            parameter.Add("@StudentCode", _accountID.StudentID);
            //
            var results = await _dbConnection.QueryAsync<StudentEnrollment>("ADDStudentEnrollment", parameter, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();



        }

        public async Task<StudentEnrollment> GetCourse(StudentEnrollment Course)
        {

            var parameter = new DynamicParameters();

            parameter.Add("@CourseCode", Course.CourseCode);
            
            var CourseNameResult = await _dbConnection.QueryAsync<StudentEnrollment>("GetCourseNameByCourseID", parameter, commandType: CommandType.StoredProcedure);

            return CourseNameResult.SingleOrDefault();



        }


    }
}
