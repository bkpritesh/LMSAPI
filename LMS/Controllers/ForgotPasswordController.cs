using Data.Repositary;
using Data.Services;
using LMS.Utility;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Model;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IForgotPasswordService _forgotPasswordService;

        private readonly IWebHostEnvironment _hostingEnvironment;
       
        public ForgotPasswordController(IForgotPasswordService forgotPasswordService, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _forgotPasswordService = forgotPasswordService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword( ForgotPassword email )
        {
            try
            {
                var ResetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
                var emailExists = await _forgotPasswordService.IsEmailExists(email.Email, ResetToken);

                if (emailExists!=null)
                { 
                    var senderEmail = email.Email;
                    var subject = "Password Reset Request";

                    var path = Path.Combine(_hostingEnvironment.ContentRootPath, "EmailTemplate", "PasswordReset.html");


                    //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplate", "PasswordReset.html");
                    var message = await System.IO.File.ReadAllTextAsync(path);


                  //  var message = System.IO.File.ReadAllText(@"~/EmailTemplate/PasswordReset.html");
                    var resetPasswordLink = _configuration.GetValue<string>("ResetPasswordLink");
                  
                   
                    message = message.Replace("{ResetPasswordLink}", resetPasswordLink+"/ForgortPassword/ResetPassword/RestToken=?"+ ResetToken);
                    bool isEmailSent = SendEmail.EmailSend(senderEmail, subject, message, null);
             

                    if (isEmailSent)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return BadRequest("Failed to send password reset link to your email address.");
                    }
                    return Ok(emailExists);

                }

                return BadRequest(false);
            }
            catch (Exception ex)
            {
            
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
         }






    }
}
