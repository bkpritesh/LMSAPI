using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Data.Services;
using System;
using System.Threading.Tasks;
using Model;
using NLog.Fluent;
using NLog.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Xml.Linq;

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
        public async Task<ActionResult<Batch>> CreateBatch(Batch Batch)
        {
            try
            {
                var newBatch = await _batchService.CreateBatch(Batch);
                return Ok(newBatch);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }



            var Student = new StudentBatch
            {
                BatchCode = Batch.BatchCode,
                StudentCode = Batch.Students
               
            };



            
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







    }
}
