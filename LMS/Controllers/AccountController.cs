using Data.Repositary;
using Data.Services;
using LMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        private readonly IAccountService _accountService;
        private readonly ICommanUtility _commanUtility;
        public AccountController(IAccountService accountService, ICommanUtility commanUtility, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _commanUtility = commanUtility;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
           
           var DecryptPassword = _commanUtility.EncryptPassword(model.Password);
            var models = new AuthenticateRequest
            {
                Email= model.Email,
                Password=DecryptPassword
            };


            var response = await _accountService.Authenticate(models);
            if (response.AccountId == null)
            {
                return NoContent();
            }

            return Ok(response);




         

        }



        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var response = await _accountService.RefreshToken(refreshToken);
            return Ok(response);
        }


    }
}
