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
		private readonly ICourse _courseService;

		public FilterCourseController(IFilterCourse FIlterCourse, ICourse courseService)
		{
			_Filtercourse = FIlterCourse;
			_courseService = courseService;
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

        [HttpGet]
		public async Task<IActionResult> GetCourseDetails(string CourseCode)
		{
			CourseSyllabus courseSyllabus = new CourseSyllabus();
			courseSyllabus.Course = await _courseService.GetCourseByID(CourseCode);
			courseSyllabus.chapters = await _Filtercourse.GetCourseDetails(CourseCode);
			return Ok(courseSyllabus);
		}
	}
}
