using Dapper;
using Data.Repositary;
using Microsoft.Extensions.Configuration;
using Model;
using Model.Courses;
using Model.Students;
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


        public async Task<IEnumerable<StudentFNameandLnameBinding>> GetStudent()
        {
            var results = await _dbConnection.QueryAsync<StudentFNameandLnameBinding>("SELECT [UGUID], [Email] ,[FName] ,[MName] ,[LName] ,[Address] ,[State] ,[City] ,[Country] ,[ContactNo] ,[Education] ,[SkillSet] ,[BirthDate] ,[JoiningDate] ,[ProfileImg] ,[StudentCode] FROM [dbo].[TBLUserDetail] where IsStudent='true'");

            var studentsWithFullName = results.Select(s => new StudentFNameandLnameBinding
            {
                UGUID = s.UGUID,
                Email = s.Email,
                FName = s.FName,
                MName = s.MName,
                LName = s.LName,
                Address = s.Address,
                State = s.State,
                City = s.City,
                Country = s.Country,
                ContactNo = s.ContactNo,
                Education = s.Education,
                SkillSet = s.SkillSet,
                BirthDate = s.BirthDate,
                JoiningDate = s.JoiningDate,
                ProfileImg = s.ProfileImg,
                StudentCode = s.StudentCode,
                FullName = $"{s.FName} {s.LName}"
            });


            //var studentsWithFullName = results.Select(s => new StudentFNameandLnameBinding
            //{
            //    UGUID = s.UGUID,
            //    Email = s.Email,
            //    FName = s.FName,
            //    MName = s.MName,
            //    LName = s.LName,
            //    Address = s.Address,
            //    State = s.State,
            //    City = s.City,
            //    Country = s.Country,
            //    ContactNo = s.ContactNo,
            //    Education = s.Education,
            //    SkillSet = s.SkillSet,
            //    BirthDate = s.BirthDate,
            //    JoiningDate = s.JoiningDate,
            //    ProfileImg = s.ProfileImg,
            //    StudentCode = s.StudentCode
               
            //})
            //.Select(s => new StudentFNameandLnameBinding
            //{
            //    UGUID = s.UGUID,
            //    Email = s.Email,
            //    FName = s.FName,
            //    MName = s.MName,
            //    LName = s.LName,
            //    Address = s.Address,
            //    State = s.State,
            //    City = s.City,
            //    Country = s.Country,
            //    ContactNo = s.ContactNo,
            //    Education = s.Education,
            //    SkillSet = s.SkillSet,
            //    BirthDate = s.BirthDate,
            //    JoiningDate = s.JoiningDate,
            //    ProfileImg = s.ProfileImg,
            //    StudentCode = s.StudentCode,
            //    FullName = $"{s.FName} {s.LName}"
            //});

            return studentsWithFullName;
        }


        //public async Task<IEnumerable<Student>> GetStudent()
        //{
        //    var results = await _dbConnection.QueryAsync<Student>("GetStudent", commandType: CommandType.StoredProcedure);
        //    return results;
        //}

        //public async Task<Student> GetStudentByID(int id)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@StudentId", id);

        //    var results = await _dbConnection.QueryAsync<Student>("GetStudent", parameters, commandType: CommandType.StoredProcedure);
        //    return results.FirstOrDefault();
        //}

        //public async Task<Student> AddStudent(Student student)
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


        //public async Task<Student> UpdateStudent(Student student)
        //{

        //    var parameters = new DynamicParameters();
        //    parameters.Add("StudentID", student.StudentID);
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

        //public async Task<IEnumerable<Student>> DeleteStudent(int id)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@StudentId", id);

        //    var results = await _dbConnection.QueryAsync<Student>("DeleteStudentByID", parameters, commandType: CommandType.StoredProcedure);
        //    return results;
        //}





        public async Task<GetStudentEnrolledInCourse> GetEnrolledCourseByStudentID(string StudentCode)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@StudentCode", StudentCode);
            var result = await _dbConnection.QueryFirstOrDefaultAsync<GetStudentEnrolledInCourse>("GetEnrolledCourseByStudentID", parameter, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
