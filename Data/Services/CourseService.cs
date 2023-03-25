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
    public class CourseService:ICourse
    {


        private readonly IDbConnection _dbConnection;

        public CourseService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<Course>> GetCourse()
        {
            var results = await _dbConnection.QueryAsync<Course>("GetCourse", commandType: CommandType.StoredProcedure);
            return results;
        }
        public async Task<Course> GetCourseByID(string CourseCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CourseCode", CourseCode);

            var results = await _dbConnection.QueryAsync<Course>("GetCourse", parameters, commandType: CommandType.StoredProcedure);
            return results.FirstOrDefault();
        }

        public async Task<AddCourse> AddCourse(AddCourse course)
        {


            var NewCId = await _dbConnection.ExecuteScalarAsync<string>("SELECT TOP 1 CourseCode FROM TBLCourse ORDER BY CourseCode DESC");

            int NewCategoryId;
            if (!string.IsNullOrEmpty(NewCId))
            {
                if (!int.TryParse(NewCId.Substring(5), out NewCategoryId))
                {
                    NewCategoryId = 0;
                }
            }
            else
            {
                NewCategoryId = 0;
            }
            var nextCategoryId = $"CID-{NewCategoryId + 1:D4}";


            var parameters = new DynamicParameters();
            parameters.Add("@CourseCode", nextCategoryId);
            parameters.Add("@CategoryCode", course.CategoryCode);
            parameters.Add("@CourseName", course.CourseName);
            parameters.Add("@Description", course.Description);
            parameters.Add("@Level", course.Level);
            parameters.Add("@CourseFee", course.CourseFee);
            parameters.Add("@IsFree", course.IsFree);
            parameters.Add("@SkillTags", course.SkillTags);
            parameters.Add("@Lectures", course.Lectures);
            parameters.Add("@DurationWeek", course.DurationWeek);



            var results = await _dbConnection.QueryAsync<AddCourse>("AddCourse", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }

        public async Task<Course> UpdateCourse(Course Ccourse)
        {
            var parameters = new DynamicParameters();
       
            parameters.Add("@CourseCode", Ccourse.CategoryCode);
            parameters.Add("@CourseName", Ccourse.CourseName);
            parameters.Add("@Description", Ccourse.Description);
            parameters.Add("@Level", Ccourse.Level);
            parameters.Add("@CourseFee", Ccourse.CourseFee);
            parameters.Add("@IsFree", Ccourse.IsFree);
            parameters.Add("@SkillTags", Ccourse.SkillTags);
            parameters.Add("@Lectures", Ccourse.Lectures);
            parameters.Add("@DurationWeek", Ccourse.DurationWeek);
            var results = await _dbConnection.QueryAsync<Course>("UpdateCourse", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }


        public async Task<IEnumerable<Course>> DeleteCourse(string CourseCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CourseId", CourseCode);

            var results = await _dbConnection.QueryAsync<Course>("DeleteCourse", parameters, commandType: CommandType.StoredProcedure);
            return results;
        }


    }
}
