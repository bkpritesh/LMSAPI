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
            var batch = await _batchService.GetBatch();
            return Ok(batch);
        }

        //[HttpGet("BatchCode")]
        //public async Task<IActionResult> GetBatchByID(string BatchCode)
        //{
        //    var batch = await _batchService.GetBatchByID(BatchCode);

        //    if (batch != null)
        //    {
        //        batch.BatchCode = BatchCode;
        //    }
        //    if (batch == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(batch);w
        //}
        [HttpGet("{batchCode}")]
        public async Task<ActionResult<GetBatch>> GetBatchByID(string batchCode)
        {
            var getBatch = await _batchService.GetBatchByID(batchCode);

            if (getBatch == null)
            {
                return NotFound();
            }

            return Ok(getBatch);
        }


        [HttpGet("GetLastBatchID")]
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
                var studentsJson = batch.Students;



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


        [HttpGet("course/{courseCode}")]
   
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
