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
    public class BatchDetailService : IBatchDetail
    {
        private readonly IDbConnection _dbConnection;

        public BatchDetailService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }



        public async Task<BDWithChapter> CreateBatchDetail(BDWithChapter batchDetails,ChatperBinding chatper)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@CourseCode", batchDetails.CourseCode);
            parameters.Add("@BatchCode", batchDetails.BatchCode);
     
            parameters.Add("@ChapterCode", chatper.ChapterCode);
            parameters.Add("@ChapterName", chatper.ChapterName);
            parameters.Add("@ChapterDescription", chatper.ChapterDescription);
            parameters.Add("@ExpectedDate", chatper.ExpectedDate);

            var results = await _dbConnection.QueryAsync<BDWithChapter>("AddBatchDetails", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }

    }
}