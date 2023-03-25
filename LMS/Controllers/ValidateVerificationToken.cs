using Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateVerificationTokenController : ControllerBase
    {
        private readonly ValidateVerificationTokenService _validateVerificationTokenService;

        public ValidateVerificationTokenController(ValidateVerificationTokenService validateVerificationTokenService)
        {
            _validateVerificationTokenService = validateVerificationTokenService;
        }

        [HttpGet("ValidateVerificationToken/{VerificationToken}")]
        public async Task<IActionResult> ValidateVerificationToken(string VerificationToken)
        {
            var isVerified = await _validateVerificationTokenService.ValidateVerificationToken(VerificationToken);

            if (isVerified)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(false);
            }
        }
    }
}
