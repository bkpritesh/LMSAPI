using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class StateandCitiesServices : IStateandCities
    {
        private readonly IDbConnection _dbConnection;


        public StateandCitiesServices(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<dynamic>> GetState()
        {
            var results = await _dbConnection.QueryAsync("SELECT [Id],[Name] FROM tblStates");
            return results;
        }



        public async Task<IEnumerable<dynamic>> GetCitiesByStateId(int stateId)
        {

            var results = await _dbConnection.QueryAsync("GetCitiesByStateId", new { stateId }, commandType: CommandType.StoredProcedure);

            return results;
        }
    }
}
