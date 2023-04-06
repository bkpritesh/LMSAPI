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
        //private readonly IDbConnection _dbConnection;
        public RegisterController(IRegisterService RgService, IWebHostEnvironment hostingEnvironment, 
            IConfiguration configuration, IAccountID accountID,ICommanUtility commanUtility,
            IUserDetail userDetail,IStudentEnrollment studentEnrollment,
            IBIllPayment billPayment)
            //IDbConnection dbConnection)
        {
            _RgService = RgService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _accountID = accountID;
            _commanUtility = commanUtility;
            _userDetail= userDetail;
            _studentEnrollment = studentEnrollment;
            _BillPayment= billPayment;
            //var connectionString = configuration.GetConnectionString("SqlConnection");
            //_dbConnection = new SqlConnection(connectionString);
      
        }


        [HttpPost]
        public async Task<IActionResult> AddAccount([FromBody] Account account)
        {


            try
            {
                var AccountID = Guid.NewGuid();
                var VerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
                _accountID.AccountId = AccountID;
                account.VerificationToken = VerificationToken;

                var result = await _RgService.AddAccount(account);


                var senderEmail = account.Email;
                var subject = "Password Reset Request";
                // reading The HTML file from the directory using the _hostingEnvironment
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "EmailTemplate", "AccountConfirmation.html");
                // to read that path and convert it into the Mesafe 
                var message = await System.IO.File.ReadAllTextAsync(path);
                var VerificationLink = _configuration.GetValue<string>("VerificationTokenLink");


                message = message.Replace("{UserFullName}", account.FirstName + account.LastName)
                                    .Replace("{ConfirmationLink}", "/verifyEmail/" + VerificationToken)
                                    .Replace("{Username}", account.FirstName)
                                    .Replace("{Password}", account.PasswordHash);
                bool isEmailSent = SendEmail.EmailSend(senderEmail, subject, message, null);

                if (isEmailSent)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(false);
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Student")]
        public async Task<IActionResult> StudentAdmission(RequestRegister requestRegister)
        {

            {
            
                var password = "test@123";
                var encrypted = _commanUtility.EncryptPassword(password);
                var VerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

                var AccountID = Guid.NewGuid();
                _accountID.AccountId = AccountID;
                var account = new Account
                {
                    VerificationToken= VerificationToken,
                    FirstName = requestRegister.FName,
                    LastName = requestRegister.FName,
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
                        Country = requestRegister.Country,
                        ContactNo = requestRegister.ContactNo,
                        Education = requestRegister.Education,
                        SkillSet = requestRegister.SkillSet,
                        BirthDate = requestRegister.BirthDate,
                        JoiningDate = requestRegister.JoiningDate
                    };

                    var UD = await _userDetail.AddUserDetail(UserDetails);
                }
              

                var StudEnrolment = new StudentEnrollment
                { 
                 CategoryCode=requestRegister.CategoryCode,
                 CourseCode=requestRegister.CourseCode, 
                 CourseFees= requestRegister.CourseFees, 
                 Discount=requestRegister.Discount,
                 TotalFees = requestRegister.CourseFees - requestRegister.Discount,


            };
                var StudR = await _studentEnrollment.Enrollment(StudEnrolment);


                var bill = new BillPayment
                {
                   Amount=requestRegister.PaidAmount,
                   CourseCode=requestRegister.CourseCode,


                };

                var Billpayment = await _BillPayment.BillPayment(bill);





                try
                {
                    var ResetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
                    var emailExists = requestRegister.Email;

                    if (emailExists != null)
                    {
                        var senderEmail = requestRegister.Email;
                        var subject = "Addmission Confrimation ";

                        var path = Path.Combine(_hostingEnvironment.ContentRootPath, "EmailTemplate", "StudentAdmission.html");

                        
                        //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplate", "PasswordReset.html");
                        var message = await System.IO.File.ReadAllTextAsync(path);

                        

                        //  var message = System.IO.File.ReadAllText(@"~/EmailTemplate/PasswordReset.html");
                        var resetPasswordLink = _configuration.GetValue<string>("ResetPasswordLink");

                        //var CourseNameResult = await .QueryAsync("GetCourseNameByCourseID", new { requestRegister.CourseCode }, commandType: CommandType.StoredProcedure);
                        //var CourseName = CourseNameResult.FirstOrDefault();

                        var CourseCode = new StudentEnrollment
                        {
                            CourseCode = requestRegister.CourseCode,

                        };
                        var CourseName = await _studentEnrollment.GetCourse(CourseCode);

                        message = message.Replace("{ResetPasswordLink}", resetPasswordLink + "/ForgortPassword/ResetPassword/RestToken=?" + ResetToken)
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
                        return Ok(emailExists);

                    }

                    return BadRequest(false);
                }
                catch (Exception ex)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
                // var StudentEnrolment = await _studentEnrollmen(UserDetails);
                // Account Id= GUID
                // IsStudent
                // Student Code - 
                // 1. Account Insert 
                // 2. userdestkpp


                // Check ISStudent 
                // If Student  get Last STudent Code and create New 
                // Is Paid then add Billing Record 
                // INsert StudentEnrollment and BillingPayment and UsersDetails
                // Send Email 
                // ProfileImg Set Detailt Image Path 

                return Ok();
            }

        }
    }
}