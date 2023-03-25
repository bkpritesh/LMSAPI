using Data.Repositary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {

        private readonly IResetPassword _ResetPassword;

        public ResetPasswordController(IResetPassword resetpassword)
        {
            _ResetPassword = resetpassword;
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            try
            {
                bool success = await _ResetPassword.ResetPassword(resetPassword);
                return Ok(new { success });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
             catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, "An error occurred while resetting the password.");
            }
        }
    }
}
