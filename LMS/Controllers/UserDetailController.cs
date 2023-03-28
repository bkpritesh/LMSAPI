using Data.Repositary;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Students;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailController : ControllerBase
    {
        private readonly IUserDetail _userDetailService;

        public UserDetailController(IUserDetail userDetailService)
        {
            _userDetailService = userDetailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetails>>> GetUserDetail()
        {
            var results = await _userDetailService.GetUserDetail();
            return Ok(results);
        }


        [HttpPost]
        public async Task<ActionResult<UserDetails>> AddUserDetail(UserDetails userDetails)
        {
            var result = await _userDetailService.AddUserDetail(userDetails);
            return Ok(result);
        }

    }
}
