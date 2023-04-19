using Data.Repositary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Model;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using LMS.Utility;
using Model.Students;
using Data.Services;
using System.Security.Principal;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ICommanUtility _commanUtility;
        private readonly IAccountID _accountID;
        private readonly IRegisterService _RgService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IUserDetail _userDetail;
        private readonly IStudentEnrollment _studentEnrollment;
        private readonly IBIllPayment _BillPayment;
 
        public RegisterController(IRegisterService RgService, IWebHostEnvironment hostingEnvironment, 
            IConfiguration configuration, IAccountID accountID,ICommanUtility commanUtility,
            IUserDetail userDetail,IStudentEnrollment studentEnrollment,
            IBIllPayment billPayment)
      
        {
            _RgService = RgService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _accountID = accountID;
            _commanUtility = commanUtility;
            _userDetail= userDetail;
            _studentEnrollment = studentEnrollment;
            _BillPayment= billPayment;
         
        }

  
        //[HttpPost]
        //public async Task<IActionResult> AddAccount([FromBody] Account account)
        //{


        //    try
        //    {
        //        var AccountID = Guid.NewGuid();
        //        var VerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        //        _accountID.AccountId = AccountID;
        //        account.VerificationToken = VerificationToken;

        //        var result = await _RgService.AddAccount(account);


        //        var senderEmail = account.Email;
        //        var subject = "Password Reset Request";
        //        // reading The HTML file from the directory using the _hostingEnvironment
        //        var path = Path.Combine(_hostingEnvironment.ContentRootPath, "EmailTemplate", "AccountConfirmation.html");
        //        // to read that path and convert it into the Mesafe 
        //        var message = await System.IO.File.ReadAllTextAsync(path);
        //        var VerificationLink = _configuration.GetValue<string>("VerificationTokenLink");


        //        message = message.Replace("{UserFullName}", account.FirstName + account.LastName)
        //                            .Replace("{ConfirmationLink}", "/verifyEmail/" + VerificationToken)
        //                            .Replace("{Username}", account.FirstName)
        //                            .Replace("{Password}", account.PasswordHash);
        //        bool isEmailSent = SendEmail.EmailSend(senderEmail, subject, message, null);

        //        if (isEmailSent)
        //        {
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return BadRequest(false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

        [HttpPost("Student")]
        public async Task<IActionResult> StudentAdmission(RequestRegister requestRegister)
        {

            {
            
                var password = "test@123";
                var encrypted = _commanUtility.EncryptPassword(password);
                var VerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

                //Reset token stored to the IAccount Serives 
                var ResetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
                _accountID.RestToken = ResetToken;
                


                var AccountID = Guid.NewGuid();
                _accountID.AccountId = AccountID;


                var account = new Account
                {
                    VerificationToken= VerificationToken,
                    FirstName = requestRegister.FName,
                    LastName = requestRegister.LName,
                    Email = requestRegister.Email,
                    PasswordHash = encrypted,
                    DisplayName = requestRegister.FName + requestRegister.LName,
                    Address = requestRegister.Address,
                    Skills = requestRegister.SkillSet,
                    AccountType= requestRegister.AccountType,
                   


                };
                var accountId = await _RgService.AddAccount(account);

                if (requestRegister.IsStudent)
                {
                    var UserDetails = new UserDetails
                    {
                  
                        IsStudent = requestRegister.IsStudent,

                        Email = requestRegister.Email,
                        FName = requestRegister.FName,
                        MName = requestRegister.MName,
                        LName = requestRegister.LName,
                        Address = requestRegister.Address,
                        City = requestRegister.City,
                        State=  requestRegister.State,
                        Country = requestRegister.Country,
                        ContactNo = requestRegister.ContactNo,
                        Education = requestRegister.Education,
                        SkillSet = requestRegister.SkillSet,
                        BirthDate = requestRegister.BirthDate,
                        JoiningDate = requestRegister.JoiningDate,
                       
                       
                    };

                    var UD = await _userDetail.AddUserDetail(UserDetails);
                }


                var StudEnrolment = new StudentEnrollment
                {
                    CategoryCode = requestRegister.CategoryCode,
                    CourseCode = requestRegister.CourseCode,
                    CourseFees = requestRegister.CourseFees,
                    Discount = requestRegister.Discount,
                    TotalFees = requestRegister.CourseFees - requestRegister.Discount,


                };
                var StudR = await _studentEnrollment.Enrollment(StudEnrolment);

                if (requestRegister.IsPaid)
                {
                    var bill = new BillPayment
                    {
                        Amount = requestRegister.PaidAmount,
                        CourseCode = requestRegister.CourseCode,
                        IsPaid = requestRegister.IsPaid,


                    };

                    var Billpayment = await _BillPayment.BillPayment(bill);

                }



                try
                {
                  
                    var emailExists = requestRegister.Email;

                    if (emailExists != null)
                    {
                        var senderEmail = requestRegister.Email;
                        var subject = "Addmission Confrimation ";

                        var path = Path.Combine(_hostingEnvironment.ContentRootPath, "EmailTemplate", "StudentAdmission.html");

                        
                      
                        var message = await System.IO.File.ReadAllTextAsync(path);                      

                     
                        var resetPasswordLink = _configuration.GetValue<string>("ResetPasswordLink");

                    
                        var CourseCode = new StudentEnrollment
                        {
                            CourseCode = requestRegister.CourseCode,

                        };
                        var CourseName = await _studentEnrollment.GetCourse(CourseCode);

                        message = message.Replace("{ResetPasswordLink}", resetPasswordLink + "/resetpassword?RestToken=" + ResetToken)
                                         .Replace("[Course Name]",CourseName)
                                         .Replace("[Address]", requestRegister.Address)
                                         .Replace("[Student Name]",requestRegister.FName+requestRegister.LName) ;

                        bool isEmailSent = SendEmail.EmailSend(senderEmail, subject, message, null);


                        if (isEmailSent)
                        {
                            return Ok(true);
                        }
                        else
                        {
                            return BadRequest(false);
                        }

                    }

                    return BadRequest(false);
                }
                catch (Exception ex)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }       
                
            }

        }









        [HttpPut("{AccountId}")]
        public async Task<IActionResult> UpdateStudetsDetails(string AccountId, [FromBody] UserDetails userDetails)
        {
            if (AccountId != null)
            {
                userDetails.AccountId = AccountId;
            }
            if (AccountId != userDetails.AccountId)
            {
                return BadRequest("The category ID in the URL doesn't match the one in the request body.");
            }

            var updateStudetsDetails = await _userDetail.UpdateStudetsDetails(userDetails);

  
            return Ok(updateStudetsDetails);
        }








        [HttpGet("StudentCode")]
        public async Task<IActionResult> GetStudentDetailById(string StudentCode)
        {
            var Course = await _userDetail.GetStudentDetailsByID(StudentCode);

            if (Course == null)
            {
                return NotFound();
            }

            return Ok(Course);
        }






    }
}