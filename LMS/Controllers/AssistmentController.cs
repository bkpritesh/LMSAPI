using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Assistment;
using NLog.Fluent;
using NLog.Web;
using OfficeOpenXml;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssistmentController : ControllerBase
    {


        private readonly AssistmentService _assistmentService;

        private static NLog.Logger Log = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        static AssistmentController()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public AssistmentController(AssistmentService assistmentService)
        {
            _assistmentService = assistmentService;
        }







        [HttpPost]
        public async Task<ActionResult> ImportAssessmentFromExcel([FromForm] AssesstmentCodeANDCourseCode file)
        {


            try
            {
                using (var package = new ExcelPackage(file.File.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // assuming the data is on the first worksheet
                    var rowCount = worksheet.Dimension.Rows;
                    var AssesstmentCode = await _assistmentService.GetAssesstmentCode();

                    for (int i = 2; i <= rowCount; i++) // assuming the first 2 rows contain headers and assessment details
                    {
                        var assessment = new Assessment
                        {
                   
                            QuestionId = worksheet.Cells[i, 1].Value?.ToString(),
                            QuestionText = worksheet.Cells[i, 2].Value?.ToString(),
                            Option1 = worksheet.Cells[i, 3].Value?.ToString(),
                            Option2 = worksheet.Cells[i, 4].Value?.ToString(),
                            Option3 = worksheet.Cells[i, 5].Value?.ToString(),
                            Option4 = worksheet.Cells[i, 6].Value?.ToString(),
                            CorrectAnswer = worksheet.Cells[i, 7].Value?.ToString()
                        };

                        // pass the question details to your service or database for processing
                        await _assistmentService.CreateAssessment(assessment,file, AssesstmentCode);
                    }

                }

                // return a success response
                return Ok();
            }
            catch (Exception ex)
            {
                // return an error response
                Log.Error(ex, "Error importing assessment from Excel.");
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("CourseCode")]
        public async Task<IActionResult> GetAssessmentByCourseCode(string CourseCode)
        {
            var Course = await _assistmentService.GetAssesstmentByCourseId(CourseCode);

            if (Course == null)
            {
                return NotFound();
            }

            return Ok(Course);
        }



        [HttpGet("AssessmentCode")]
        public async Task<IActionResult> GetQuestionsByAssesstmentId(string AssessmentCode)
        {
            var Assestment = await _assistmentService.GetQuestionsByAssesstmentId(AssessmentCode);

            if (Assestment == null)
            {
                return NotFound();
            }

            return Ok(Assestment);
        }








        //[HttpPost("submit")]

        //    public async Task<ActionResult<int>> SubmitQuiz([FromBody] Dictionary<string, string> quizData, AssesstANDStudCode aNDStudCode)

        ////public async Task<ActionResult<int>> SubmitQuiz([FromBody] Dictionary<string, string> quizData, AssesstANDStudCode aNDStudCode)
        //{
        //    try
        //    {
        //        // Create an instance of AssesstANDStudCode from the input data
        //        var assessmentAndStudent = new AssesstANDStudCode
        //        {
        //            AssessmentCode = aNDStudCode.AssessmentCode,
        //
        //            = aNDStudCode.StudentCode,
        //        };

        //        int score = await _assistmentService.SubmitQuizResults(quizData, assessmentAndStudent);
        //        return Ok(score);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


     

        [HttpPost("submit")]
        public async Task<ActionResult<ExamResult>> SubmitQuiz([FromBody] SubmitQuizModel model)
        {
            try
            {
                ExamResult score = await _assistmentService.SubmitQuizResults(model.QuizData, model.AssesstANDStudCode);
                return Ok(score);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}
