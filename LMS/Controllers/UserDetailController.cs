using Data.Repositary;
using Data.Services;
using LMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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


        

        private readonly IAccount _accountId;
        private readonly IUserDetail _userDetailService;
        private readonly IRegisterService _RgService;
        public UserDetailController(IUserDetail userDetailService,IRegisterService RgService, IAccount accountId)
        {
            _userDetailService = userDetailService;
            _RgService = RgService;
            _accountId = accountId;
  
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetails>>> GetUserDetail()
        {
            var results = await _userDetailService.GetUserDetail();
            return Ok(results);
        }


        [HttpPost]
        //public async Task<ActionResult<UserDetails>> AddUserDetail(UserDetails userDetails)
        //{
        //    var result = await _userDetailService.AddUserDetail(userDetails);
        //    return Ok(result);
        //}



        //public async Task<ActionResult<UserDetails>> AddUserDetail(UserDetails userDetails)
        //{
        //    userDetails.AccountId = new Account { 
        //    };
        //    var result = await _userDetailService.AddUserDetail(userDetails);
        //    return Ok(result);
        //}

        [HttpPost]
        public async Task<ActionResult<UserDetails>> AddUserDetail(UserDetails userDetails)
        {
            var account = new Account { /* populate account properties here */ };
            var accountId = await _RgService.AddAccount(account);
            userDetails.AccountId = accountId.ToString();
            var result = await _userDetailService.AddUserDetail(userDetails);
            return Ok(result);
        }


    }
}
