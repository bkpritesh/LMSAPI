using Data.Repositary;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterCourseController : ControllerBase
    {
        private readonly IFilterCourse _Filtercourse;

        public FilterCourseController(IFilterCourse FIlterCourse)
        {
            _Filtercourse = FIlterCourse;
        }

        //[HttpGet("filter")]
        //public async Task<IActionResult> FilterCourses(string categoryCodes = null, string courseName = null, int? level = null, string skills = null, bool? isFree = null)
        //{

        //    var courses = await _Filtercourse.FilterCourse(categoryCodes, courseName, level, skills, isFree);
        //    return Ok(courses);
        //}


        [HttpPost("CourseFilter")]
        public async Task<IActionResult> FilterCourses(FilterCourse Fcourse)
        {
            var courses = await _Filtercourse.FilterCourses(Fcourse);
            return Ok(courses);
        }
    }
}
