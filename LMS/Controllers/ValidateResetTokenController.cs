using Data.Repositary;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Data;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateResetTokenController : ControllerBase
    {
        private readonly IValidateResetToken _validateResetToken;
        private readonly IConfiguration _configuration;

        public ValidateResetTokenController(IValidateResetToken validateResetToken, IConfiguration configuration)
        {
            _validateResetToken = validateResetToken;
            _configuration = configuration;
        }

    


        [HttpGet("ResetPassword/{resetToken}")]
        public async Task<IActionResult> ValidateToken(string resetToken)
        {
            try
            {
                var (isValid, isExpired) = await _validateResetToken.ValidateResetToken(resetToken);

                if (!isValid)
                {
                    return BadRequest(false);
                }

                if (isExpired)
                {
                    return BadRequest(false);
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
