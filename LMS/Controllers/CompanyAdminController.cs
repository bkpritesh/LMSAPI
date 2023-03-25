using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyAdminController : ControllerBase
    {


        [HttpPost]
        public async Task<Student> AddCompanyAdmin(CompanyAdmin CAdmin)
        {
            var result = await _.CreateCategory(category);
            return Ok(result);
        }

    }
}
