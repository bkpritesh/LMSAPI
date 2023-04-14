using Dapper;
using Data.Repositary;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Model.Assistment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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








        public async Task<Assessment> CreateAssessment(Assessment assessment,AssesstmentCodeANDCourseCode file,string AssesstmentCode)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@AssessmentCode", AssesstmentCode);
            parameters.Add("@CourseCode", file.CourseCode);
            parameters.Add("@Question", assessment.QuestionText);
            parameters.Add("@Option1", assessment.Option1);
            parameters.Add("@Option2", assessment.Option2);
            parameters.Add("@Option3", assessment.Option3);
            parameters.Add("@Option4", assessment.Option4);
            parameters.Add("@CorrectAnswer", assessment.CorrectAnswer);

            var results = await _dbConnection.QueryAsync<Assessment>("AddAssesstment", parameters, commandType: CommandType.StoredProcedure);
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
 