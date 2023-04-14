using Dapper;
using Data.Repositary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model;
using Newtonsoft.Json;
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
        public async Task<IEnumerable<GetBatch>> GetBatch()
        {
            var query = "SELECT TBLBatch.*, TBLCourse.CourseName FROM TBLBatch JOIN TBLCourse ON TBLBatch.CourseCode = TBLCourse.CourseCode WHERE TBLBatch.CourseCode = TBLCourse.CourseCode;";
            var results = await _dbConnection.QueryAsync<GetBatch>(query);

            return results;
        }

        //public async Task<GetBatch> GetBatchByID(string BatchCode)
        //{

        //    var results = await _dbConnection.QueryAsync<GetBatch>("SELECT * FROM [LMS].[dbo].[TBLBatch] where BatchCode = @BatchCode", new { BatchCode = BatchCode });
        //    return results.FirstOrDefault();
        //}



        //public async Task<GetBatch> GetBatdchcByID(string BatchCode)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@BatchCode", BatchCode);

        //    var results = await _dbConnection.QueryAsync<GetBatch>("GetBatchDetails", parameters, commandType: CommandType.StoredProcedure);

        //    var studentList = new List<Student>();

        //    foreach (var result in results)
        //    {
        //        // extract student code and student name from the Students string
        //        var students = result.Students.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        //        foreach (var student in students)
        //        {
        //            var studentInfo = student.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        //            var studentCode = studentInfo[0];
        //            var studentName = studentInfo[1];

        //            // add student code and student name to the list
        //            studentList.Add(new Student { StudentCode = studentCode, StudentName = studentName });
        //        }

        //        // convert student list to json and store in the Students property
        //        result.Students = JsonConvert.SerializeObject(studentList);
        //    }

        //    return results;
        //}



        public async Task<GetBatch> GetBatchByID(string batchCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BatchCode", batchCode);

            var results = await _dbConnection.QueryAsync<GetBatch, NAME, GetBatch>("GetBatchDetails", (batch, name) =>
            {
                batch.Students ??= new List<NAME>();
                if (!batch.Students.Any(s => s.StudentCode == name.StudentCode))
                {
                    batch.Students.Add(name);
                }
                return batch;
            },
            parameters,
            commandType: CommandType.StoredProcedure,
            splitOn: "StudentCode,StudentCode");

            var studentList = results
                .SelectMany(r => r.Students)
                .Select(s => new { StudentCode = s.StudentCode, FullName = s.FullName })
                .Distinct()
                .Select(s => new NAME { StudentCode = s.StudentCode, FullName = s.FullName })
                .ToList();

            var getBatchWithStudents = new GetBatch
            {
                BatchCode = results.FirstOrDefault()?.BatchCode ?? string.Empty,
                BatchName = results.FirstOrDefault()?.BatchName ?? string.Empty,
                CourseCode = results.FirstOrDefault()?.CourseCode ?? string.Empty,
                CourseName = results.FirstOrDefault()?.CourseName ?? string.Empty,
                Assessment = results.FirstOrDefault()?.Assessment ?? 0,
                Description = results.FirstOrDefault()?.Description ?? string.Empty,
                StartTIme = results.FirstOrDefault()?.StartTIme ?? string.Empty,
                EndTIme = results.FirstOrDefault()?.EndTIme ?? string.Empty,

                InstructorCode = results.FirstOrDefault()?.InstructorCode ?? string.Empty,
                Students = studentList
            };

            // serialize the GetBatchWithStudents model to JSON
            var json = JsonConvert.SerializeObject(getBatchWithStudents);

            return getBatchWithStudents;
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
           // parameters.Add("@BatchTime", Batch.BatchTime);
      
            parameters.Add("@Assessment", Batch.Assessment);

            parameters.Add("@Description",Batch.Description);
            parameters.Add("@StartTime", Batch.StartTime);
            parameters.Add("@EndTime", Batch.EndTime);
            parameters.Add("@InstructorCode", Batch.InstructorCode);
            var results = await _dbConnection.QueryAsync<Batch>("AddBatch", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }



        public async Task<Batch> UpdateBatch(Batch Batch)
        {


            var parameters = new DynamicParameters();


            parameters.Add("@BatchCode", Batch.BatchCode);
            parameters.Add("@BatchName", Batch.BatchName);
            parameters.Add("@CourseCode", Batch.CourseCode);
         
            parameters.Add("@Assessment", Batch.Assessment);
            parameters.Add("@Description", Batch.Description);
            parameters.Add("@StartTime", Batch.StartTime);
            parameters.Add("@EndTime", Batch.EndTime);
            parameters.Add("@InstructorCode", Batch.InstructorCode);

            var results = await _dbConnection.QueryAsync<Batch>("UpdateBatch", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();

        }



        public async Task<StudentBatch> AddStudentBatch(StudentBatch Batch)
        {


            var parameters = new DynamicParameters();


            parameters.Add("@BatchCode", Batch.BatchCode);
        

            parameters.Add("@Student", Batch.StudentCode);
        

            var results = await _dbConnection.QueryAsync<StudentBatch>("UpdateStudentBatch", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();

        }


        public async Task<IEnumerable<dynamic>> GetCoureNameByBCID(string CourseCode)
        {

            var results = await _dbConnection.QueryAsync("GETCoursenamebyBatchCourseCode", new { CourseCode }, commandType: CommandType.StoredProcedure);
            return results;
        }























    }
}
