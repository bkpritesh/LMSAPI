using Data.Repositary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
                    

        private readonly IEducation _EducationService;

        public EducationController (IEducation education)
        {
            _EducationService = education;
        }

        

        [HttpGet("Qualification")]
        public async Task<IActionResult> GetEducaton()
        {
            var education = await _EducationService.GetEducaton();
            return Ok(education);
        }

        [HttpGet("skills")]
        public async Task<IActionResult> GetSkills()
        {
            var education = await _EducationService.GetSkills();
            return Ok(education);
        }



    }
}
