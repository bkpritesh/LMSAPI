using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
  public class ResetPasswordService:IResetPassword
    {
        private readonly IDbConnection _dbConnection;

        public ResetPasswordService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }


        public async Task<bool> ResetPassword(ResetPassword resetPassword)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ResetToken", resetPassword.ResetToken);
            //parameters.Add("@PasswordHash", resetPassword.Password);

            // output parameter to receive the AccountId from the stored procedure
            parameters.Add("@AccountId", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

            // Execute the CheckTheAccountID stored procedure to get the AccountId
            await _dbConnection.ExecuteAsync("CheckTheAccountID", parameters, commandType: CommandType.StoredProcedure);

            // Get the AccountId from the output parameter
            string accountId = parameters.Get<string>("@AccountId");






            // Check if AccountId is valid
            if (accountId == "0")
            {
                throw new ArgumentException("Reset token is invalid or password update failed.");
            }

            // Update the user password using the AccountId
            var updateParams = new DynamicParameters();
            updateParams.Add("@AccountId", accountId);
            updateParams.Add("@PasswordHash", resetPassword.Password);

            int rowsAffected = await _dbConnection.ExecuteAsync("UpdateUserPasswordByAccoubtId", updateParams, commandType: CommandType.StoredProcedure);

            if (rowsAffected == 0)
            {
                throw new ArgumentException("Failed to update password.");
            }

            return true;
        }







    }
}
