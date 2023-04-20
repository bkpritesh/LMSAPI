using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model.Students;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Services
{
    public class DashBoardService : IDashBoard
    {
        private readonly IDbConnection _dbConnection;

        public DashBoardService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }


        public async Task<int> GetStudentCount()
        {
            var result = await _dbConnection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM tblUserDetail WHERE IsStudent = 'true';");
            return result;
        }

        public async Task<int> GetInstructorCount()
        {
            var result = await _dbConnection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM tblUserDetail WHERE IsInstructor = 'true';");
            return result;
        }

        public async Task<int> GetAdminCount()
        {
            var result = await _dbConnection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM tblUserDetail WHERE IsAdmin = 'true';");
            return result;
        }

        public async Task<int> GetCourseCount()
        {
            var result = await _dbConnection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM TBlCourse");
            return result;
        }



        public async Task<IEnumerable<dynamic>> GetCountOfStudentInCourse(int year)
        {        
              
                var results = await _dbConnection.QueryAsync("SELECT c.[CourseName], COUNT(se.[EId]) AS [NumberOfStudentsEnrolled] FROM [dbo].[TBLCourse] c LEFT JOIN [dbo].[TBLStudentEnrollment] se ON c.[CourseCode] = se.[CourseCode] WHERE YEAR(se.[CreatedDate]) = @Year GROUP BY c.[CourseName] ORDER BY c.[CourseName];", new { Year = year });
                return results;
            
        }



    }
}