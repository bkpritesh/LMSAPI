using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model;
using System.Data;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Data.Services
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IDbConnection _dbConnection;

        public ForgotPasswordService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }
        //public async Task<Account> IsEmailExists(string email)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@Email", email);

        //    var result = await _dbConnection.QuerySingleOrDefaultAsync<Account>("CheckEmailExists", parameters, commandType: CommandType.StoredProcedure);
        //    if (result != null)
        //    {

        //        // token generation 
        //        var ResetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        //        // sending this Generated token to the database
        //        var updateParameters = new DynamicParameters();
        //        updateParameters.Add("@Email", email);
        //        updateParameters.Add("@ResetToken", ResetToken);
        //        await _dbConnection.ExecuteAsync("UpdateToken", updateParameters, commandType: CommandType.StoredProcedure);
        //    }
        //    return result;
                
           
        //}


        public async Task<Account> IsEmailExists(string email, string ResetToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);

            var result = await _dbConnection.QuerySingleOrDefaultAsync<Account>("CheckEmailExists", parameters, commandType: CommandType.StoredProcedure);
            if (result != null)
            {
                // sending this Generated token to the database
                var updateParameters = new DynamicParameters();
                updateParameters.Add("@Email", email);
                updateParameters.Add("@ResetToken", ResetToken);
                await _dbConnection.ExecuteAsync("UpdateToken", updateParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }
     


    }
}
