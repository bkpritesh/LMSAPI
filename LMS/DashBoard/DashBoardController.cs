using Data.Repositary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DashBoard;

namespace LMS.DashBoard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoard _dashBoard;

        public DashBoardController(IDashBoard dashBoard)
        {
            _dashBoard = dashBoard;
        }

        [HttpGet("GetAllCount")]
        public async Task<IActionResult> GetAllCount()
        {
            // Get student count
            var studentCount = await _dashBoard.GetStudentCount();

            // Get instructor count
            var instructorCount = await _dashBoard.GetInstructorCount();

            // Get admin count
            var adminCount = await _dashBoard.GetAdminCount();

            // Get course count
            var courseCount = await _dashBoard.GetCourseCount();

            // Return the counts as JSON
            return new JsonResult(new Dashboard
            {
                StudentCount = studentCount,
                InstructorCount = instructorCount,
                AdminCount = adminCount,
                CourseCount = courseCount
            });
        }



        //[HttpGet("GetCourseStudentCount")]
        //public async Task<IActionResult> GetCourseStudentCount()
        //{
        //    try
        //    {
        //        var results = await _dashBoard.GetCountOfStudentInCourse();
        //        return Ok(results);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any errors and return an appropriate response
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }
        //}

        [HttpGet("{year}")]
        public async Task<IActionResult> GetEnrollmentCountByYear(int year)
        {
            try
            {
                // Call the GetCountOfStudentInCourse method to get the enrollment count for the specified year
                var enrollmentCounts = await _dashBoard.GetCountOfStudentInCourse(year);
                return Ok(enrollmentCounts);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an error response
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("GetCourseCountByYM/{year}")]
        public async Task<IActionResult> GetCourseCountByYM(int year)
        {
            try
            {
                // Call the GetCountOfStudentInCourse method to get the enrollment count for the specified year
                var CourseCountByYM = await _dashBoard.GetCourseCountByYM(year);
                return Ok(CourseCountByYM);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an error response
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetPaymentDetailsByYear/{year}")]
        public async Task<IActionResult> GetPaymentDetailsByYear(int year)
        {
            try
            {
                // Call the GetCountOfStudentInCourse method to get the enrollment count for the specified year
                var CourseCountByYM = await _dashBoard.GetPayment(year);
                return Ok(CourseCountByYM);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an error response
                return StatusCode(500, ex.Message);
            }
        }

    }
}
