using Dapper;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Batchs;
using NLog.Web;
using OfficeOpenXml;
using System;
using System.Globalization;
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

        //[HttpPost]
        //public async Task<ActionResult> CreateBatchDetail([FromForm] BDWithChapter batch)
        //{
        //    try
        //    {
        //        using (var package = new ExcelPackage(batch.File.OpenReadStream()))
        //        {
        //            var worksheet = package.Workbook.Worksheets[0]; // assuming the data is on the first worksheet
        //            var rowCount = worksheet.Dimension.Rows;

        //            for (int i = 2; i <= rowCount; i++) // assuming the first row contains headers
        //            {
        //                var chapterCode = worksheet.Cells[i, 1].Value?.ToString();
        //                var chapterName = worksheet.Cells[i, 2].Value?.ToString();
        //                var chapterDescription = worksheet.Cells[i, 3].Value?.ToString();
        //                var expectedDateString = worksheet.Cells[i, 4].Value?.ToString();
        //                var expectedDate = DateTime.ParseExact(expectedDateString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        //               // var expectedDate = DateTime.ParseExact(expectedDateString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        //                // create a new ChapterBinding object
        //                var chapter = new ChatperBinding
        //                {
        //                    ChapterCode = chapterCode,
        //                    ChapterName = chapterName,
        //                    ChapterDescription = chapterDescription,
        //                    ExpectedDate = expectedDate.Date
        //                };

        //                // pass the chapter details along with the batch details to your service or database for processing
        //                await _batchDetailService.CreateBatchDetail(batch, chapter);
        //            }
        //        }

        //        // return a success response
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        // return an error response
        //        Log.Error(ex, "Error creating batch details.");
        //        return BadRequest(ex.Message);
        //    }
        //}


        //[HttpPost]
        //public async Task<ActionResult> CreateBatchDetail([FromForm] BDWithChapter batch)
        //{
        //    try
        //    {
        //        using (var package = new ExcelPackage(batch.File.OpenReadStream()))
        //        {
        //            var worksheet = package.Workbook.Worksheets[0]; // assuming the data is on the first worksheet
        //            var rowCount = worksheet.Dimension.Rows;

        //            // List of format strings to try when parsing the date
        //            var dateFormats = new List<string>
        //                   {
        //                "dd-MM-yyyy HH:mm:ss",
        //                "yyyy-MM-ddTHH:mm:ss",
        //                "yyyy-MM-ddTHH:mm:ssZ",
        //                "yyyy-MM-dd HH:mm:ss",
        //                "yyyy/MM/dd HH:mm:ss",
        //                "dd/MM/yyyy HH:mm:ss",
        //                "dd-MMM-yyyy HH:mm:ss",
        //                "dd MMM yyyy HH:mm:ss",
        //                "MM/dd/yyyy HH:mm:ss",
        //                "MMM dd, yyyy HH:mm:ss",
        //                "dd-MM-yyyy",
        //                "yyyy-MM-dd",
        //                "yyyy/MM/dd",
        //                "dd/MM/yyyy",
        //                "dd-MMM-yyyy",
        //                "dd MMM yyyy",
        //                "MM/dd/yyyy",
        //                "dd-MM-yyyy",
        //                "MMM dd, yyyy"
        //            };

        //            for (int i = 2; i <= rowCount; i++) // assuming the first row contains headers
        //            {
        //                var chapterCode = worksheet.Cells[i, 1].Value?.ToString();
        //                var chapterName = worksheet.Cells[i, 2].Value?.ToString();
        //                var chapterDescription = worksheet.Cells[i, 3].Value?.ToString();
        //                var expectedDateString = worksheet.Cells[i, 4].Value?.ToString();

        //                DateTime expectedDate;
        //                if (!DateTime.TryParseExact(expectedDateString, dateFormats.ToArray(), CultureInfo.InvariantCulture, DateTimeStyles.None, out expectedDate))
        //                {
        //                    throw new ArgumentException($"Invalid date format: {expectedDateString}");
        //                }

        //                var expectedDateFormatted = expectedDate.ToString("dd/MM/yyyy");
        //                // create a new ChapterBinding object
        //                var chapter = new ChatperBinding
        //                {
        //                    ChapterCode = chapterCode,
        //                    ChapterName = chapterName,
        //                    ChapterDescription = chapterDescription,
        //                    ExpectedDate = DateTime.ParseExact(expectedDateFormatted, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        //                    //ExpectedDate = expectedDate.Date
        //                };

        //                // pass the chapter details along with the batch details to your service or database for processing
        //                await _batchDetailService.CreateBatchDetail(batch, chapter);
        //            }
        //        }

        //        // return a success response
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        // return an error response
        //        Log.Error(ex, "Error creating batch details.");
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult> CreateBatchDetail([FromForm] BDWithChapter batch)
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
                        var expectedDateString = worksheet.Cells[i, 4].Value?.ToString();

                        // get the current user's culture
                        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

                        // get the culture used in the Excel sheet
                        CultureInfo excelCulture = new CultureInfo("en-GB"); // replace with the actual culture used in your Excel sheet

              
                        var chapter = new ChatperBinding
                        {
                            ChapterCode = chapterCode,
                            ChapterName = chapterName,
                            ChapterDescription = chapterDescription,
                            ExpectedDate = DateTime.Parse(expectedDateString)
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




        [HttpPut("BatchCode")]
        public async Task<IActionResult> UpdateBatchDetail(string BatchCode, [FromBody] BatchDetails batch)
        {
            if (BatchCode != batch.BatchCode)
            {
                return BadRequest();
            }

            var update = await _batchDetailService.UpdateBatchDetail(batch);



            return Ok(update);
        }


        [HttpPut("{batchCode}/{chapterCode}")]
        public async Task<IActionResult> UpdateBatchDetail( string batchCode, string chapterCode, [FromBody] BatchDetails batch)
        {
            if ( batchCode != batch.BatchCode || chapterCode != batch.ChapterCode)
            {
                return BadRequest();
            }

            var updatedBatch = await _batchDetailService.UpdateBatchDetail(batch);

            return Ok(updatedBatch);
        }


        [HttpGet("{BatchCode}")]
        public async Task<IActionResult> GetStudentByBCode(string BatchCode)
        {
            try
            {
                var batcheStudent  = await _batchDetailService.GetStudentByBCode(BatchCode);
                return Ok(batcheStudent);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the database operation
                return StatusCode(500, $"Error retrieving batches: {ex.Message}");
            }
        }




        [HttpGet("GetDetailByBCode")]
        public async Task<IActionResult> GetDetailByBCode(string? BatchCode)
        {
            try
            {
                var batcheStudent = await _batchDetailService.GetDetailByBCode(BatchCode);
                return Ok(batcheStudent);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the database operation
                return StatusCode(500, $"Error retrieving batches: {ex.Message}");
            }
        }



        [HttpGet("GetDetailByBCHCode")]
        public async Task<IActionResult> GetDetailByBCHCode(string? BatchCode, string? chapterCode)
        {
            try
            {
                var batcheStudent = await _batchDetailService.GetDetailByBCHCode(BatchCode, chapterCode);
                return Ok(batcheStudent);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the database operation
                return StatusCode(500, $"Error retrieving batches: {ex.Message}");
            }
        }





    }
}
