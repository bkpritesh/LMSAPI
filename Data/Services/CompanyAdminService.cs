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
    public class CompanyAdminService : ICompanyAdmin
    {
        private readonly IDbConnection _dbConnection;

        public CompanyAdminService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<Student> AddCompanyAdmin(CompanyAdmin CAdmin)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName",    Rs.CompanyId);
            parameters.Add("@LastName",     Rs.FirstName);
            parameters.Add("@EmailID",      Rs.LastName);
            parameters.Add("@City",         Rs.Skills);
            parameters.Add("@DateOfBirth",  Rs.Email);
            parameters.Add("@Address",      Rs.Address);
      

            var results = await _dbConnection.QueryAsync<Student>("AddCompany", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }

    }
}
