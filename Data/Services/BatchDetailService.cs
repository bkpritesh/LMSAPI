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


        public async Task<BatchDetails> UpdateBatchDetail(BatchDetails model)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@BatchCode", model.BatchCode);
            parameters.Add("@ChapterCode", model.ChapterCode);
            parameters.Add("@ExpectedDate", model.ExpectedDate);
            parameters.Add("@CompletionDate", model.CompletionDate);
            parameters.Add("@IsCompleted", model.IsCompleted);
            parameters.Add("@PresentStudent", model.PresentStudent);
            parameters.Add("@AbsentStudent", model.AbsentStudent);
            parameters.Add("@MeetingLink", model.MeetingLink);
            parameters.Add("@RecordingLink", model.RecordingLink);
            parameters.Add("@Resource", model.Resource);
            parameters.Add("@ModifiedDate", model.ModifiedDate);
            parameters.Add("@ModifiedBy", model.ModifiedBy);

            var results = await _dbConnection.QueryAsync<BatchDetails>("UpdateBatchDetails", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }




        public async Task<IEnumerable<dynamic>> GetStudentByBCode(string Bcode)
        {
            var results = await _dbConnection.QueryAsync("[GetStudentByBatchCode]", new { Batchcode = Bcode }, commandType: CommandType.StoredProcedure);
            return results;
        }


        public async Task<IEnumerable<dynamic>> GetDetailByBCHCode(string Bcode, string chapterCode)
        {
            var results = await _dbConnection.QueryAsync("SELECT * FROM [dbo].[BatchDetails] WHERE BatchCode = @BatchCode AND ChapterCode = @ChapterCode", new { BatchCode = Bcode, ChapterCode = chapterCode });
            return results;
        }


    }
}