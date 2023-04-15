using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class FilterCourseService : IFilterCourse
    {
        private readonly IDbConnection _dbConnection;

        public FilterCourseService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }


        public async Task<List<FilterCourse>> FilterCourses(FilterCourse Fcourse)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryCodes", Fcourse.CategoryCode);
            parameters.Add("@CourseName", Fcourse.CourseName);
            parameters.Add("@Level", Fcourse.Level);
            parameters.Add("@Skills", Fcourse.SkillTags);
            parameters.Add("@IsFree", Fcourse.IsFree);

            var courses = await _dbConnection.QueryAsync<FilterCourse>("FilterCourse", parameters, commandType: CommandType.StoredProcedure);
            return courses.ToList();
        }

    }
}
