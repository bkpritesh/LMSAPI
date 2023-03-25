using Data.Repositary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var response = _accountService.Authenticate(model);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public ActionResult<AuthenticateResponse> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _accountService.RefreshToken(refreshToken);
            return Ok(response);
        }
    }
}
