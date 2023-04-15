using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model;
using Model.Assistment;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class AssistmentService : IAssistment
    {

        private readonly IDbConnection _dbConnection;



        public AssistmentService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
        }



         //  for getting the documentPath to store in the next parameter

        //var results = await _dbConnection.QueryAsync("GetDocumentPathByDocID", new { DocID }, commandType: CommandType.StoredProcedure);
        //return results;

             //
            //
           //   GETTING ALL THE ASSESSTMENT BY THE COURSE ID 
          //
         // 
        public async Task<dynamic> GetAssesstmentByCourseId(string CourseCode)
        {
            var results = await _dbConnection.QueryAsync("SELECT [AssessmentCode] ,[AssessmentName] ,[CourseCode] FROM [LMS].[dbo].[TBLAssessmentQuestions] where CourseCode = @CourseCode ", new { CourseCode= CourseCode } );

            return results;
        }


            //
           //
          //   GETTING ALL THE QUESTITIONS  BY THE ASSESTEMNT 
         //
        // 
        public async Task<dynamic> GetQuestionsByAssesstmentId(string AssessmentCode)
        {
            var results = await _dbConnection.QueryAsync("SELECT [QuestionId],[Question],[Option1],[Option2]\r\n      ,[Option3]\r\n      ,[Option4]FROM [LMS].[dbo].[TBLAssessmentQuestions] where [AssessmentCode] = @AssessmentCode ", new { AssessmentCode = AssessmentCode });

            return results;
        }












        public async Task<Assessment> CreateAssessment(Assessment assessment,AssesstmentCodeANDCourseCode file,string AssesstmentCode)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@AssessmentCode", AssesstmentCode);
            parameters.Add("@AssessmentName", file.AssessmentName);
            parameters.Add("@CourseCode", file.CourseCode);
            parameters.Add("@QuestionId", assessment.QuestionId);
            parameters.Add("@Question", assessment.QuestionText);
            parameters.Add("@Option1", assessment.Option1);
            parameters.Add("@Option2", assessment.Option2);
            parameters.Add("@Option3", assessment.Option3);
            parameters.Add("@Option4", assessment.Option4);
            parameters.Add("@CorrectAnswer", assessment.CorrectAnswer);



            var results = await _dbConnection.QueryAsync<Assessment>("[dbo].[AddAssessment]", parameters, commandType: CommandType.StoredProcedure);
            return results.SingleOrDefault();
        }





        public async Task<string> GetAssesstmentCode()
        {
            var AsiCode = await _dbConnection.ExecuteScalarAsync<string>("SELECT TOP 1 AssessmentCode FROM TBLAssessmentQuestions ORDER BY AssessmentCode DESC");

            int NewAssesstmentCode;
            if (!string.IsNullOrEmpty(AsiCode))
            {
                if (!int.TryParse(AsiCode.Substring(5), out NewAssesstmentCode))
                {
                    NewAssesstmentCode = 0;
                }
            }
            else
            {
                NewAssesstmentCode = 0;
            }
            var NextAssesstmentCode = $"AS-{NewAssesstmentCode + 1:D4}";

            return NextAssesstmentCode;
        }

    }
}
 