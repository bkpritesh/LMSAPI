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
    public  class EducationService : IEducation
    {
        private readonly IDbConnection _dbConnection;


        public EducationService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<dynamic>> GetEducaton()
        {
            var results = await _dbConnection.QueryAsync("SELECT [EducationID],[Education]FROM [TBLEducations]");
            return results;
        }
        public async Task<IEnumerable<dynamic>> GetSkills ()
        {
            var results = await _dbConnection.QueryAsync("SELECT [SkillId],[Skills] FROM [TBLSkills]");
            return results;
        }
    }
}
