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
    public  class InquiriesService : IInquiries
    {

        private readonly IDbConnection _dbConnection;

        public InquiriesService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<Inquiries> InsertInquiries(Inquiries inquiries)
        {


            var uniqueId = DateTime.Now.ToString("yyyyMMddHHmmssfff");

     


            var parameters = new DynamicParameters();
            parameters.Add("@Id", uniqueId);
            parameters.Add("@Name", inquiries.Name);
            parameters.Add("@Email", inquiries.Email);
            parameters.Add("@ContactNo", inquiries.ContactNo);
            parameters.Add("@Purpose", inquiries.Purpose);
            parameters.Add("@WhatsAppNo", inquiries.WhatsAppNo);
            parameters.Add("@Message", inquiries.Message);
            parameters.Add("@IsLead", inquiries.IsLead);

            var results = await _dbConnection.QueryAsync<Inquiries>("[InsertInquiry]", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }

    }
}
