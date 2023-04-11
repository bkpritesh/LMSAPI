using Data.Repositary;
using LMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly ICommanUtility _commanUtility;
        private readonly IResetPassword _ResetPassword;

        public ResetPasswordController(IResetPassword resetpassword,ICommanUtility commanUtility)
        {
            _ResetPassword = resetpassword;
            _commanUtility = commanUtility;
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            try
            {
                resetPassword.Password = _commanUtility.EncryptPassword(resetPassword.Password);
                var Reset = new ResetPassword { 
                
                Password= resetPassword.Password,
                ResetToken =resetPassword.ResetToken,    
                };
                bool success = await _ResetPassword.ResetPassword(Reset);
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
