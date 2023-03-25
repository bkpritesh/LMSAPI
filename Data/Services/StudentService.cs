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
    public class StudentService:IStudentService
    {
        private readonly IDbConnection _dbConnection;


        public StudentService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<Student>> GetStudent()
        {
            var results = await _dbConnection.QueryAsync<Student>("GetStudent", commandType: CommandType.StoredProcedure);
            return results;
        }

        public async Task<Student> GetStudentByID(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StudentId", id);

            var results = await _dbConnection.QueryAsync<Student>("GetStudent", parameters, commandType: CommandType.StoredProcedure);
            return results.FirstOrDefault();
        }

        public async Task<Student> AddStudent(Student student)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", student.FirstName);
            parameters.Add("@LastName", student.LastName);
            parameters.Add("@EmailID", student.EmailID);
            parameters.Add("@DateOfBirth", student.DateOfBirth);
            parameters.Add("@Address", student.Address);
            parameters.Add("@City", student.City);
            parameters.Add("@PinCode", student.PinCode);
            parameters.Add("@State", student.State);
            parameters.Add("@Country", student.Country);
            parameters.Add("@MobileNo", student.MobileNo);
            parameters.Add("@Qualification", student.Qualification);
            parameters.Add("@CourseName", student.CourseName);

            var results = await _dbConnection.QueryAsync<Student>("AddStudent", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }


        public async Task<Student> UpdateStudent(Student student)
        {
            
            var parameters = new DynamicParameters();
            parameters.Add("StudentID", student.StudentID);
            parameters.Add("@FirstName", student.FirstName);
            parameters.Add("@LastName", student.LastName);
            parameters.Add("@EmailID", student.EmailID);
            parameters.Add("@DateOfBirth", student.DateOfBirth);
            parameters.Add("@Address", student.Address);
            parameters.Add("@City", student.City);
            parameters.Add("@PinCode", student.PinCode);
            parameters.Add("@State", student.State);
            parameters.Add("@Country", student.Country);
            parameters.Add("@MobileNo", student.MobileNo);
            parameters.Add("@Qualification", student.Qualification);
            parameters.Add("@CourseName", student.CourseName);

            var results = await _dbConnection.QueryAsync<Student>("AddStudent", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }

        public async Task<IEnumerable<Student>> DeleteStudent(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StudentId", id);

            var results = await _dbConnection.QueryAsync<Student>("DeleteStudentByID", parameters, commandType: CommandType.StoredProcedure);
            return results;
        }
    }
}
