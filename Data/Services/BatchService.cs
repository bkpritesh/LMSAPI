using Dapper;
using Data.Repositary;
using Microsoft.AspNetCore.Mvc;
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
    public  class BatchService : IBatch
    {
        private readonly IDbConnection _dbConnection;

        public BatchService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }
        public async Task<IEnumerable<Batch>> GetBatch()
        {
            var query = "SELECT * FROM TBLBatch";
            var results = await _dbConnection.QueryAsync<Batch>(query);
            return results;
        }

       



        public async Task<ActionResult<string>> GetLastBatchID()
        {
            var BID = await _dbConnection.ExecuteScalarAsync<string>("SELECT TOP 1 BatchCode FROM TBLBatch ORDER BY BatchCode DESC");

            int NewBatchId;
            if (!string.IsNullOrEmpty(BID))
            {
                if (!int.TryParse(BID.Substring(5), out NewBatchId))
                {
                    NewBatchId = 0;
                }
            }
            else
            {
                NewBatchId = 0;
            }
            var NextBatchId = $"B-{NewBatchId + 1:D4}";

            return (NextBatchId);
        }




        public async Task<Batch> CreateBatch(Batch Batch)
        {


            var BID = await _dbConnection.ExecuteScalarAsync<string>("SELECT TOP 1 BatchCode FROM TBLBatch ORDER BY BatchCode DESC");

            int NewBatchId;
            if (!string.IsNullOrEmpty(BID))
            {
                if (!int.TryParse(BID.Substring(5), out NewBatchId))
                {
                    NewBatchId = 0;
                }
            }
            else
            {
                NewBatchId = 0;
            }
            var NextBatchId = $"B-{NewBatchId + 1:D4}";
                

            var parameters = new DynamicParameters();
            parameters.Add("@BatchCode", NextBatchId);
            parameters.Add("@BatchName", Batch.BatchName);
            parameters.Add("@CourseCode", Batch.CourseCode);
            parameters.Add("@BatchTime", Batch.BatchTime);

            parameters.Add("@Assessment", Batch.Assessment);

            parameters.Add("@Description",Batch.Description);

            var results = await _dbConnection.QueryAsync<Batch>("AddBatch", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }



        public async Task<Batch> UpdateBatch(Batch Batch)
        {


            var parameters = new DynamicParameters();


            parameters.Add("@BatchCode", Batch.BatchCode);
            parameters.Add("@BatchName", Batch.BatchName);
            parameters.Add("@CourseCode", Batch.CourseCode);
            parameters.Add("@BatchTime", Batch.BatchTime);
            parameters.Add("@Assessment", Batch.Assessment);
            parameters.Add("@Description", Batch.Description);

            var results = await _dbConnection.QueryAsync<Batch>("UpdateBatch", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }



























    }
}
