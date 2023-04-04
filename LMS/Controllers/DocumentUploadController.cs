
using Dapper;
using Data.Repositary;
using Data.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Model;
using System.Data;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentUploadController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IDocument _Document;

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IDbConnection _dbConnection;


        public DocumentUploadController(IDocument document, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            _dbConnection = new SqlConnection(connectionString);
            _Document = document;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }




        //[HttpPost]
        //public async Task<IActionResult> UploadFile(IFormFile file, string AccountId,string DocumentType)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("No file was selected for upload.");
        //    }
        //    // Giving the file naame 
        //    var fileName = Path.GetFileName(file.FileName);
        //    // path where the file to be storeed 
        //    var uploadsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "Images");
        //    if (!Directory.Exists(uploadsPath))
        //    {
        //        // creating the directory
        //        Directory.CreateDirectory(uploadsPath);
        //    }

        //    var filePath = Path.Combine(uploadsPath, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    //passing the filepath to the services 
        //    var result = await _Document.AddDocument(filePath, DocumentType,AccountId );

        //    if (result < 0)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add document to database.");
        //    }



        //[HttpPost]
        //public async Task<IActionResult> UploadFile(IFormFile file, string AccountId, string DocumentType)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("No file was selected for upload.");
        //    }
        //    // Giving the file naame 
        //    var fileName = Path.GetFileName(file.FileName);
        //    // path where the file to be storeed 
        //    var uploadsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "Images");
        //    if (!Directory.Exists(uploadsPath))
        //    {
        //        // creating the directory
        //        Directory.CreateDirectory(uploadsPath);
        //    }

        //    var filePath = Path.Combine(uploadsPath, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }


        //    //passing the filepath to the services 
        //    var result = await _Document.AddDocument(filePath, DocumentType, AccountId);

        //    if (result < 0)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }









            [HttpPost]
            public async Task<IActionResult> UploadFile(IFormFile file, string AccountId, string DocumentType)
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file was selected for upload.");
                }

                // Giving the file name 
                var fileName = Path.GetFileName(file.FileName);

                // Path where the file to be stored 
                var uploadsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads" , AccountId,"DocumentType");

                switch (DocumentType)
                {
                    case "CategoryImage":
                        uploadsPath = Path.Combine(uploadsPath, "CategoryImages");
                        break;
                    case "CourseImage":
                        uploadsPath = Path.Combine(uploadsPath, "CourseImages");
                        break;
                    case "ProfileImage":
                        uploadsPath = Path.Combine(uploadsPath, "ProfileImages");
                        break;
                    case "Admission":
                        uploadsPath = Path.Combine(uploadsPath, "Admissions");
                        break;
                    case "CourseCircular":
                        uploadsPath = Path.Combine(uploadsPath, "CourseCirculars");
                        break;
                    case "LessonPlan":
                        uploadsPath = Path.Combine(uploadsPath, "LessonPlans");
                        break;
                    default:
                        return BadRequest("Invalid document type.");
                }

                if (!Directory.Exists(uploadsPath))
                {
                    // Creating the directory
                    Directory.CreateDirectory(uploadsPath);
                }

                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Adding the document to the database
                var result = await _Document.AddDocument(filePath, DocumentType, AccountId);

                if (result < 0)
                {
                // Document added successfully, returning file path and document information
                return Ok(new { FilePath = filePath, Document = DocumentType });
             
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            





























            // [HttpPost]
            //public async Task<IActionResult> UploadFile([FromBody] byte[] file, string documentType)
            //{
            //    if (file == null || file.Length == 0)
            //    {
            //        return BadRequest("No file was selected for upload.");
            //    }

            //    // Generate 
            //    var fileName = Path.GetRandomFileName();

            //    // Create the full path to the file
            //    var uploadsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "Images");
            //    if (!Directory.Exists(uploadsPath))
            //    {
            //        Directory.CreateDirectory(uploadsPath);
            //    }
            //    var filePath = Path.Combine(uploadsPath, fileName);

            //    // Write the file to disk
            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await stream.WriteAsync(file, 0, file.Length);
            //    }


            //    // Add the file to the database
            //    var result = await _Document.AddDocument(filePath, documentType);

            //    if (result < 0)
            //    {
            //          return Ok();
            //    }
            //    else
            //    {
            //        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add document to database.");
            //    }






        }

    }



}