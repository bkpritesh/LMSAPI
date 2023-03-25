using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model;

namespace Data.Services
{
    public class ValidateResetTokenService : IValidateResetToken
    {

        private readonly IDbConnection _dbConnection;

        public ValidateResetTokenService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }

 

        public async Task<(bool isValid, bool isExpired)> ValidateResetToken(string resetToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ResetToken", resetToken);
            parameters.Add("@IsValid", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@IsExpired", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            var results = await _dbConnection.ExecuteAsync("CheckResetToken", parameters, commandType: CommandType.StoredProcedure);

            bool isValid = parameters.Get<bool>("@IsValid");
            bool isExpired = parameters.Get<bool>("@IsExpired");

            return (isValid, isExpired);
        }


    }
}
