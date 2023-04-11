using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using NLog.Web;
using OfficeOpenXml;
using System;
using System.Threading.Tasks;

namespace LMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BatchDetailController : ControllerBase
    {
        private readonly BatchDetailService _batchDetailService;

        private static NLog.Logger Log = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        static BatchDetailController()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public BatchDetailController(BatchDetailService batchDetailService)
        {
            _batchDetailService = batchDetailService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateBatchDetail([FromForm] BatchDetails batch)
        {
            try
            {
                using (var package = new ExcelPackage(batch.File.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // assuming the data is on the first worksheet
                    var rowCount = worksheet.Dimension.Rows;

                    for (int i = 2; i <= rowCount; i++) // assuming the first row contains headers
                    {
                        var chapterCode = worksheet.Cells[i, 1].Value?.ToString();
                        var chapterName = worksheet.Cells[i, 2].Value?.ToString();
                        var chapterDescription = worksheet.Cells[i, 3].Value?.ToString();
                        var expectedDate = DateTime.Parse(worksheet.Cells[i, 4].Value?.ToString());

                        // create a new ChapterBinding object
                        var chapter = new ChatperBinding
                        {
                            ChapterCode = chapterCode,
                            ChapterName = chapterName,
                            ChapterDescription = chapterDescription,
                            ExpectedDate = expectedDate
                        };

                        // pass the chapter details along with the batch details to your service or database for processing
                        await _batchDetailService.CreateBatchDetail(batch, chapter);
                    }
                }

                // return a success response
                return Ok();
            }
            catch (Exception ex)
            {
                // return an error response
                Log.Error(ex, "Error creating batch details.");
                return BadRequest(ex.Message);
            }
        }
    }
}
