using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Data.Services;
using System;
using System.Threading.Tasks;
using Model;
using NLog.Fluent;
using NLog.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Data.Repositary;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly BatchService _batchService;

        private static NLog.Logger Log = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        public BatchController(BatchService batchService)
        {
            _batchService = batchService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Log.Error("Manin");
            var course = await _batchService.GetBatch();
            return Ok(course);
        }


        [HttpGet("batchId")]
        public async Task<IActionResult> GetLastBatchID()
        {
            Log.Error("Manin");
            var LastbatchID = await _batchService.GetLastBatchID();
            return Ok(new { BatchCode = LastbatchID.Value });
        }
      
        [HttpPost]
        public async Task<ActionResult<Batch>> CreateBatch(Batch batch)
        {
            try
            {
                var newBatch = await _batchService.CreateBatch(batch);
                // Deserialize the students property from the input JSON object
                var studentsJson = "[{\"StudentCode\":\"S-0001\",\"FName\":\"string\"},{\"StudentCode\":\"S-0002\",\"FName\":\"Honey\"}]";
                    // sample JSON string
                var studentsList = JsonConvert.DeserializeObject<List<StudentBatch>>(studentsJson); // deserialize JSON string into List<Batch> object
                var studentCodes = string.Join(",", studentsList.Select(s => s.StudentCode)); // select the StudentCode properties and join them into a comma-separated string


                foreach (var student in studentsList)
                {
                    var studentBatch = new StudentBatch
                    {
                        BatchCode = batch.BatchCode,
                        StudentCode = student.StudentCode
                    };
                    await _batchService.AddStudentBatch(studentBatch);
                }

               // await _batchService.AddStudentBatch(studentBatch);

                return Ok(batch);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


























        [HttpPut("BatchCode")]
        public async Task<IActionResult> UpdateBatch(string BatchCode, [FromBody] Batch batch)
        {
            if (BatchCode != batch.BatchCode)
            {
                return BadRequest();
            }

            var update = await _batchService.UpdateBatch(batch);



            return Ok(update);
        }




        [HttpGet]
        [Route("{CourseCode}")]
        public async Task<IActionResult> GetCoureNameByBCID(string CourseCode)
        {
            var Category = await _batchService.GetCoureNameByBCID(CourseCode);

            if (Category == null)
            {
                return NotFound(false);
            }

            return Ok(Category);
        }



    }
}
