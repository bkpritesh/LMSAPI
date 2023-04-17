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
            var results = await _dbConnection.QueryAsync("SELECT [AssessmentCode] ,[AssessmentName] ,[CourseCode] FROM [LMS].[dbo].[TBLAssessmentQuestions] where CourseCode = @CourseCode ", new { CourseCode = CourseCode });

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












        public async Task<Assessment> CreateAssessment(Assessment assessment, AssesstmentCodeANDCourseCode file, string AssesstmentCode)
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






        //public int SubmitQuizResults(Dictionary<string, string> quizData)
        //{
        //    // Start a transaction
        //    SqlTransaction transaction = (SqlTransaction)_dbConnection.BeginTransaction();


        //    try
        //    {
        //        // Prepare a command to insert the quiz results into the database
        //        string insertQuery = "INSERT INTO QuizResults (Question, Answer, IsCorrect) VALUES (@Question, @Answer, @IsCorrect)";
        //        SqlCommand insertCommand = new SqlCommand(insertQuery, (SqlConnection)_dbConnection, transaction);

        //        insertCommand.Parameters.Add("@Question", SqlDbType.NVarChar);
        //        insertCommand.Parameters.Add("@Answer", SqlDbType.NVarChar);
        //        insertCommand.Parameters.Add("@IsCorrect", SqlDbType.Bit);

        //        // Loop through the quiz data and check the answers against the database
        //        int score = 0;
        //        foreach (KeyValuePair<string, string> entry in quizData)
        //        {
        //            string question = entry.Key;
        //            string answer = entry.Value;

        //            // Query the correct answer for the question
        //            string selectQuery = "SELECT [CorrectAnswer] FROM [TBLAssessmentQuestions] WHERE [Question] = @Question";
        //            SqlCommand selectCommand = new SqlCommand(selectQuery, (SqlConnection)_dbConnection, transaction);
        //            selectCommand.Parameters.AddWithValue("@Question", question);
        //            string correctAnswer = (string)selectCommand.ExecuteScalar();

        //            // Compare the answer in the JSON data with the correct answer
        //            bool isCorrect = answer.Equals(correctAnswer);

        //            // Update the score
        //            score += isCorrect ? 1 : -1;

        //            // Insert the quiz result into the database
        //            insertCommand.Parameters["@Question"].Value = question;
        //            insertCommand.Parameters["@Answer"].Value = answer;
        //            insertCommand.Parameters["@IsCorrect"].Value = isCorrect;
        //            insertCommand.ExecuteNonQuery();
        //        }

        //        // Commit the transaction
        //        transaction.Commit();

        //        return score;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Roll back the transaction if there was an error
        //        transaction.Rollback();
        //        throw ex;
        //    }
        //}


        //public async Task<int> SubmitQuizResults(Dictionary<string, string> quizData)
        //{
        //    // Start a transaction
        //    SqlConnection sqlConnection = (SqlConnection)_dbConnection;
        //    await sqlConnection.OpenAsync();


        //    // Start a transaction
        //    SqlTransaction transaction = (SqlTransaction)_dbConnection.BeginTransaction();


        //    try
        //    {
        //        int score = 0;

        //        // Loop through the quiz data and check the answers against the database
        //        foreach (KeyValuePair<string, string> entry in quizData)
        //        {
        //            string question = entry.Key;
        //            string answer = entry.Value;

        //            // Query the correct answer for the question
        //            string selectQuery = "SELECT [CorrectAnswer] FROM [TBLAssessmentQuestions] WHERE [Question] = @Question";
        //            SqlCommand selectCommand = new SqlCommand(selectQuery, (SqlConnection)_dbConnection, transaction);
        //            selectCommand.Parameters.AddWithValue("@Question", question);
        //            string correctAnswer = (string)selectCommand.ExecuteScalar();

        //            // Compare the answer in the JSON data with the correct answer
        //            bool isCorrect = answer.Equals(correctAnswer);

        //            // Update the score
        //            score += isCorrect ? 1 : -1;
        //        }

        //        // Calculate the percentage score
        //        int totalQuestions = quizData.Count;
        //        float percentage = (float)score / totalQuestions * 100;

        //        // Determine if the quiz was passed
        //        bool isPass = percentage >= 50;

        //        // Insert the quiz result into the database
        //        string insertQuery = "INSERT INTO QuizResults (Score, IsPass, TotalQuestion) VALUES (@Score, @IsPass, @TotalQuestion)";
        //        SqlCommand insertCommand = new SqlCommand(insertQuery, (SqlConnection)_dbConnection, transaction);
        //        insertCommand.Parameters.AddWithValue("@Score", score);
        //        insertCommand.Parameters.AddWithValue("@IsPass", isPass);
        //        insertCommand.Parameters.AddWithValue("@TotalQuestion", totalQuestions);
        //        await insertCommand.ExecuteNonQueryAsync();

        //        // Commit the transaction
        //        transaction.Commit();

        //        return score;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Roll back the transaction if there was an error
        //        transaction.Rollback();
        //        throw ex;
        //    }
        //}

        public async Task<int> SubmitQuizResults(Dictionary<string, string> quizData)
        {
            // Start a transaction
            SqlConnection sqlConnection = (SqlConnection)_dbConnection;
            await sqlConnection.OpenAsync();
            SqlTransaction transaction = (SqlTransaction)_dbConnection.BeginTransaction();

            try
            {
                int score = 0;

                // Loop through the quiz data and check the answers against the database
                foreach (KeyValuePair<string, string> entry in quizData)
                {
                    string question = entry.Key;
                    string answer = entry.Value;

                    // Query the correct answer for the question
                    string selectQuery = "SELECT [CorrectAnswer] FROM [TBLAssessmentQuestions] WHERE [Question] = @Question";
                    SqlCommand selectCommand = new SqlCommand(selectQuery, (SqlConnection)_dbConnection, transaction);
                    selectCommand.Parameters.AddWithValue("@Question", question);
                    string correctAnswer = (string)selectCommand.ExecuteScalar();

                    // Compare the answer in the JSON data with the correct answer
                    bool isCorrect = answer.Equals(correctAnswer);

                    // Update the score
                    score += isCorrect ? 1 : 0;
                }

                // Calculate the percentage score
                int totalQuestions = quizData.Count;
                float percentage = (float)score / totalQuestions * 100;

                // Determine if the quiz was passed
                bool isPass = percentage >= 70;

                // Insert the quiz result into the database
                string insertQuery = "INSERT INTO QuizResults (Score, IsPass, TotalQuestion) VALUES (@Score, @IsPass, @TotalQuestion)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, (SqlConnection)_dbConnection, transaction);
                insertCommand.Parameters.AddWithValue("@Score", score);
                insertCommand.Parameters.AddWithValue("@IsPass", isPass);
                insertCommand.Parameters.AddWithValue("@TotalQuestion", totalQuestions);
                await insertCommand.ExecuteNonQueryAsync();

                // Commit the transaction
                transaction.Commit();

                return score;
            }
            catch (Exception ex)
            {
                // Roll back the transaction if there was an error
                transaction.Rollback();
                throw ex;
            }
        }

        //public async Task<int> SubmitQuizResults(Dictionary<string, string[]> quizData)
        //{
        //    // Start a transaction
        //    SqlConnection sqlConnection = (SqlConnection)_dbConnection;
        //    await sqlConnection.OpenAsync();
        //    SqlTransaction transaction = (SqlTransaction)_dbConnection.BeginTransaction();

        //    try
        //    {
        //        int score = 0;

        //        // Loop through the quiz data and check the answers against the database
        //        foreach (KeyValuePair<string, string[]> entry in quizData)
        //        {
        //            string question = entry.Key;
        //            string[] answers = entry.Value;

        //            // Query the correct answer for the question
        //            string selectQuery = "SELECT [CorrectAnswer] FROM [TBLAssessmentQuestions] WHERE [Question] = @Question";
        //            SqlCommand selectCommand = new SqlCommand(selectQuery, (SqlConnection)_dbConnection, transaction);
        //            selectCommand.Parameters.AddWithValue("@Question", question);
        //            string correctAnswer = (string)selectCommand.ExecuteScalar();

        //            // Compare the answer in the JSON data with the correct answer
        //            bool isCorrect = false;
        //            foreach (string answer in answers)
        //            {
        //                if (correctAnswer.Equals(answer))
        //                {
        //                    isCorrect = true;
        //                    break;
        //                }
        //            }

        //            // Update the score
        //            score += isCorrect ? 1 : -1;
        //        }

        //        // Calculate the percentage score
        //        int totalQuestions = quizData.Count;
        //        float percentage = (float)score / totalQuestions * 100;

        //        // Determine if the quiz was passed
        //        bool isPass = percentage >= 50;

        //        // Insert the quiz result into the database
        //        string insertQuery = "INSERT INTO QuizResults (Score, IsPass, TotalQuestion) VALUES (@Score, @IsPass, @TotalQuestion)";
        //        SqlCommand insertCommand = new SqlCommand(insertQuery, (SqlConnection)_dbConnection, transaction);
        //        insertCommand.Parameters.AddWithValue("@Score", score);
        //        insertCommand.Parameters.AddWithValue("@IsPass", isPass);
        //        insertCommand.Parameters.AddWithValue("@TotalQuestion", totalQuestions);
        //        await insertCommand.ExecuteNonQueryAsync();

        //        // Commit the transaction
        //        transaction.Commit();

        //        return score;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Roll back the transaction if there was an error
        //        transaction.Rollback();
        //        throw ex;
        //    }
        //}




    
    }
}
 