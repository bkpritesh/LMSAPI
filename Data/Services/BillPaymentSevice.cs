using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model.Students;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class BillPaymentSevice : IBIllPayment
    {

        private readonly IAccountID _accountID;
        private readonly IDbConnection _dbConnection;

        public BillPaymentSevice(IConfiguration configuration, IAccountID accountID)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
            _accountID = accountID;
        }



        public async Task<BillPayment> BillPayment(BillPayment billPayment)
        {

            var parameter = new DynamicParameters();

         
            parameter.Add("@AccountID", _accountID.AccountId);
            parameter.Add("@StudentCode", _accountID.StudentID);
            parameter.Add("@Amount", billPayment.Amount);
            parameter.Add("@PaymentType", billPayment.PaymentType);
            parameter.Add("@IsPaid", billPayment.IsPaid);
            parameter.Add("@CourseCode", billPayment.CourseCode);


            //
            var results = await _dbConnection.QueryAsync<BillPayment>("InsertBillingPayment", parameter, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();



        }
    }
}
