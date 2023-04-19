 using Dapper;
using Data.Repositary;
using Microsoft.Extensions.Configuration;
using Model;
using Model.Courses;
using Model.Students;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class CourseService:ICourse
    {


        private readonly IDbConnection _dbConnection;
		private static NLog.Logger Log = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		public CourseService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<Course>> GetCourse()
        {
			Log.Error("Service");
			var results = await _dbConnection.QueryAsync<Course>("GetCourse", commandType: CommandType.StoredProcedure);
            return results;
        }

        //  for getting the documentPath to store in the next parameter
     
            //var results = await _dbConnection.QueryAsync("GetDocumentPathByDocID", new { DocID }, commandType: CommandType.StoredProcedure);
            //return results;


        public async Task<Course> GetCourseByID(string CourseCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CourseCode", CourseCode);
            var results = await _dbConnection.QueryAsync<Course>("GetCourse", parameters, commandType: CommandType.StoredProcedure);
            return results.FirstOrDefault();
        }


        public async Task<AddCourse> AddCourse(AddCourse course)
        {
             var parameter = new DynamicParameters();
             parameter.Add("@DocID", course.DocID);
            IEnumerable<string> docpath = await _dbConnection.QueryAsync<string>("GetDocumentPathByDocID", parameter, commandType: CommandType.StoredProcedure);
            string resultString = docpath.FirstOrDefault()?.ToString();

            var DocPath = resultString;

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
            parameters.Add("@CourseBanner", DocPath);
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

            var results = await _dbConnection.QueryAsync("AddCourse", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }

        public async Task<Course> UpdateCourse(Course Ccourse)
        {
            var parameters = new DynamicParameters();   
            parameters.Add("@CourseCode", Ccourse.CourseCode);
            var gettingDocbyDocID = await _dbConnection.QueryAsync<string>("[GetDocumentPathByDocID]", new { Ccourse.DocID }, commandType: CommandType.StoredProcedure);
            var DocumnentPath = gettingDocbyDocID.FirstOrDefault();
            if (DocumnentPath != null)
            {
                parameters.Add("@CourseBanner", DocumnentPath);
            }
            parameters.Add("@CategoryCode", Ccourse.CategoryCode);
     
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
            parameters.Add("@CourseCode", CourseCode);

            var results = await _dbConnection.QueryAsync<Course>("DeleteCourse", parameters, commandType: CommandType.StoredProcedure);
            return results;
        }

		public async Task<IEnumerable<Course>> CourseFilter(RequestCourseFilter course)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@param_CategoryCode", course.CategoryCode);
			parameters.Add("@param_Level", course.Level);
			parameters.Add("@param_SkillTags", course.SkillTags);
			parameters.Add("@param_Price", course.Price);
			parameters.Add("@param_CourseKeyWord", course.CourseKeyWord);
			var results = await _dbConnection.QueryAsync<Course>("usp_Course_Filter", parameters, commandType: CommandType.StoredProcedure);
            return results;
		}
	}
}
