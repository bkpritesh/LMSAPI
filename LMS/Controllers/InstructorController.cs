using Data.Repositary;
using Data.Services;
using LMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Identity.Client;
using Model;

using Model.Students;
using System.Configuration;
using System.Security.Cryptography;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _InstructorService;
        private readonly ICommanUtility _commanUtility;
        private readonly IAccountID _accountID;
        private readonly IRegisterService _RgService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IUserDetail _userDetail;
        private readonly IStudentEnrollment _studentEnrollment;
        private readonly IBIllPayment _BillPayment;
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService InstructorService, IRegisterService RgService, IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration, IAccountID accountID, ICommanUtility commanUtility,
            IUserDetail userDetail, IStudentEnrollment studentEnrollment,
            IBIllPayment billPayment, IInstructorService instructorService)
        {
            _InstructorService = InstructorService;
            _RgService = RgService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _accountID = accountID;
            _commanUtility = commanUtility;
            _userDetail = userDetail;
            _studentEnrollment = studentEnrollment;
            _BillPayment = billPayment;
            _instructorService = instructorService;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var instructors = await _InstructorService.GetInstructor();
            return Ok(instructors);
        }


        [HttpPost("Instructor")]
        public async Task<IActionResult> RegisterInstructor(RegisterInstructor registerInstructor)
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
                    VerificationToken = VerificationToken,
                    FirstName = registerInstructor.FName,
                    LastName = registerInstructor.LName,
                    Email = registerInstructor.Email,
                    PasswordHash = encrypted,
                    DisplayName = registerInstructor.FName + registerInstructor.LName,
                    Address = registerInstructor.Address,
                    Skills = registerInstructor.SkillSet,
                    AccountType = registerInstructor.AccountType,



                };
                var accountId = await _RgService.AddAccount(account);

                if (registerInstructor.IsInstructor)
                {
                    var UserDetails = new UserDetails
                    {

                        IsInstructor = registerInstructor.IsInstructor,

                        Email = registerInstructor.Email,
                        FName = registerInstructor.FName,
                        MName = registerInstructor.MName,
                        LName = registerInstructor.LName,
                        Address = registerInstructor.Address,
                        City = registerInstructor.City,
                        State = registerInstructor.State,
                        Country = registerInstructor.Country,
                        ContactNo = registerInstructor.ContactNo,
                        Education = registerInstructor.Education,
                        SkillSet = registerInstructor.SkillSet,
                        BirthDate = registerInstructor.BirthDate,
                        JoiningDate = registerInstructor.JoiningDate,


                    };

                    var UD = await _instructorService.AddInstructorDetail(UserDetails);
                }


            



            try
            {

                var emailExists = registerInstructor.Email;

                if (emailExists != null)
                {
                    var senderEmail = registerInstructor.Email;
                    var subject = "Addmission Confrimation ";

                    var path = Path.Combine(_hostingEnvironment.ContentRootPath, "EmailTemplate", "StudentAdmission.html");



                    var message = await System.IO.File.ReadAllTextAsync(path);


                    var resetPasswordLink = _configuration.GetValue<string>("ResetPasswordLink");


                    var CourseCode = new StudentEnrollment
                    {
                        CourseCode = registerInstructor.CourseCode,

                    };
                    var CourseName = await _studentEnrollment.GetCourse(CourseCode);

                    message = message.Replace("{ResetPasswordLink}", resetPasswordLink + "/resetpassword?RestToken=" + ResetToken)
                                     .Replace("[Course Name]", CourseName)
                                     .Replace("[Address]", registerInstructor.Address)
                                     .Replace("[Student Name]", registerInstructor.FName + registerInstructor.LName);

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
}






