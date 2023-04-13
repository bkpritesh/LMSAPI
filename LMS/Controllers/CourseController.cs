using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Data.Services;
using Data.Repositary;
using Model;
using Model.Courses;
using NLog.Web;

namespace LMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourse _courseService;
        private static NLog.Logger Log = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        public CourseController(ICourse courseService)
        {
            _courseService = courseService;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Log.Error("Manin");
            var course = await _courseService.GetCourse();
            return Ok(course);
        }


        [HttpGet("CourseCode")]
        public async Task<IActionResult> GetCourseByID(string CourseCode)
        {
            var Course = await _courseService.GetCourseByID(CourseCode);

            if (Course == null)
            {
                return NotFound();
            }

            return Ok(Course);
        }



        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] AddCourse course)
        {


            var result = await _courseService.AddCourse(course);
            return Ok(result);
        }



        [HttpPut("CourseCode")]
        public async Task<IActionResult> UpdateCourse(string Coursecode, [FromBody] Course course)
        {
            if (Coursecode != course.CourseCode)
            {
                return BadRequest();
            }

            var update = await _courseService.UpdateCourse(course);



            return Ok(update);
        }


        [HttpDelete("{CourseCode}")]
        public async Task<IActionResult> DeleteCourse(string CourseCode)
        {
            await _courseService.DeleteCourse(CourseCode);
            return Ok();
        }


        [HttpPost("CourseSearch")]
        public async Task<IActionResult> GetCourseFilter(RequestCourseFilter courseFilter)
        {
            var result = await _courseService.CourseFilter(courseFilter);
            return Ok(result);
        }

    }
}
