using Data.Repositary;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _InstructorService;

        public InstructorController(IInstructorService InstructorService)
        {
            _InstructorService = InstructorService;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var instructors = await _InstructorService.GetInstructor();
            return Ok(instructors);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetInstructorByID(int id)
        //{
        //    var instructor = await _InstructorService.GetInstructorByID(id);

        //    if (instructor == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(instructor);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddInstructor([FromBody]Instructor instructor)
        //{
        //    var result = await _InstructorService.AddInstructor(instructor);
        //    return Ok(result);
        //}


        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateInstructor(int id, [FromBody]Instructor instructor)
        //{
        //    if(id != null)
        //    {
        //        instructor.InstructorId = id;


        //    }
        //    if (id != instructor.InstructorId)
        //    {
        //        return BadRequest("The Instructor ID in the URL doesn't match the one in the request body.");
        //    }

        //    var updateInstructor = await _InstructorService.UpdateInstructor(instructor);

        //    if (updateInstructor == null)
        //    {
        //        return NotFound($"No Instructor found with ID {id}");
        //    }

        //    return Ok(updateInstructor);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteInstructorById(int id)
        //{
        //    await _InstructorService.DeleteInstructorById(id);

        //    return NoContent();
        //}
    }
}
