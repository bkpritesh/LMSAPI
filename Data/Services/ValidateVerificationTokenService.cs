using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public  class ValidateVerificationTokenService:IValidateVerificationToken
    {
        private readonly IDbConnection _dbConnection;

        public ValidateVerificationTokenService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }



        public async Task<bool> ValidateVerificationToken(string VerificationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VerificationToken", VerificationToken);
            parameters.Add("@IsVerified", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync("CheckVerificationToken", parameters, commandType: CommandType.StoredProcedure);

            bool isVerified = parameters.Get<bool>("@IsVerified");

            return isVerified;
        }



    }

}
