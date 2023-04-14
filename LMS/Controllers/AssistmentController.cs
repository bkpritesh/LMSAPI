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












        //[HttpPost]
        //public async Task<ActionResult> ImportAssessmentFromExcel([FromForm] Assessment model)
        //{
        //    try
        //    {
        //        using (var package = new ExcelPackage(model.OpenReadStream()))
        //        {
        //            var worksheet = package.Workbook.Worksheets[0]; // assuming the data is on the first worksheet
        //            var rowCount = worksheet.Dimension.Rows;

        //            create a new assessment
        //            var assessment = new Assessment
        //            {
        //                AssessmentCode = model.Assessment,
        //                CourseCode = model.CourseCode
        //            };

        //            pass the assessment details to your service or database for processing
        //           await _assessmentService.CreateAssessment(assessment);

        //            loop through each row and create a new question for the assessment
        //            for (int i = 2; i <= rowCount; i++) // assuming the first row contains headers
        //                    {
        //                        var question = worksheet.Cells[i, 1].Value?.ToString();
        //                        var option1 = worksheet.Cells[i, 2].Value?.ToString();
        //                        var option2 = worksheet.Cells[i, 3].Value?.ToString();
        //                        var option3 = worksheet.Cells[i, 4].Value?.ToString();
        //                        var option4 = worksheet.Cells[i, 5].Value?.ToString();
        //                        var answer = worksheet.Cells[i, 6].Value?.ToString();

        //                        create a new Question object
        //                       var questionObj = new Question
        //                       {
        //                           AssessmentCode = model.AssessmentCode,
        //                           Question = question,
        //                           Option1 = option1,
        //                           Option2 = option2,
        //                           Option3 = option3,
        //                           Option4 = option4,
        //                           Answer = answer
        //                       };

        //                        pass the question details to your service or database for processing
        //                       await _assessmentService.CreateQuestion(questionObj);
        //            }
        //        }

        //        return a success response
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return an error response
        //        Log.Error(ex, "Error importing assessment from Excel.");
        //        return BadRequest(ex.Message);
        //    }
        //}


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

                    for (int i = 3; i <= rowCount; i++) // assuming the first 2 rows contain headers and assessment details
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









    }
}
