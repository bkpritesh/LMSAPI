using Data.Repositary;
using Data.Services;
using LMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;


namespace LMS.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _StudentService;

        public StudentController(IStudentService StudentService)
        {
            _StudentService = StudentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _StudentService.GetStudent();

            return Ok(students);
        }



        //[HttpGet("StudentCode")]
        //public async Task<IActionResult> GetEnrolledCourseByStudentID(string StudentCode)
        //{
        //    var studentCode = await _StudentService.GetEnrolledCourseByStudentID(StudentCode);

        //    if (studentCode == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(studentCode);
        //}

        [HttpGet("{StudentCode}")]
        public async Task<IActionResult> GetEnrolledCourseByStudentID(string StudentCode)
        {
            var studentEnrollment = await _StudentService.GetEnrolledCoursesByStudentID(StudentCode);

            if (studentEnrollment == null)
            {
                return NotFound();
            }

            return Ok(studentEnrollment);
        }




        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetStudentByID(int id)
        //{
        //  var student = await _StudentService.GetStudentByID(id);
        //    return Ok(student);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddStudent([FromBody] Student student)
        //{


        //    var result = await _StudentService.AddStudent(student);
        //    string senderEmail = "thamas9824@gmail.com";
        //    string subject = "New Student Added";

        //    string message = System.IO.File.ReadAllText(@"D:\C# projects\LMSAPI\LMS\EmailTemplate\AccountConfirmation.html");

        //    message = message.Replace("[[StudentName]]", student.EmailID);


        //    bool isEmailSent = SendEmail.EmailSend(senderEmail, subject, message, null);




        //    return Ok(result);


        //}


        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        //{
        //    if (id != null)
        //    {
        //        student.StudentID = id;
        //    }
        //    if (id != student.StudentID)
        //    {
        //        return BadRequest("The category ID in the URL doesn't match the one in the request body.");
        //    }

        //    var updatedCategory = await _StudentService.UpdateStudent(student);

        //    if (updatedCategory == null)
        //    {
        //        return NotFound($"No category found with ID {id}");
        //    }

        //    return Ok(updatedCategory);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteStudent(int id)
        //{
        //    await _StudentService.DeleteStudent(id);

        //    return NoContent();
        //}


    }
}
