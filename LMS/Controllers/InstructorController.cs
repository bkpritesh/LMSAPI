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

        public InstructorController(IInstructorService InstructorService, IRegisterService RgService, IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration, IAccountID accountID, ICommanUtility commanUtility,
            IUserDetail userDetail, IStudentEnrollment studentEnrollment,
            IBIllPayment billPayment)
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
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var instructors = await _InstructorService.GetInstructor();
            return Ok(instructors);
        }


        //[HttpPost("Student")]
        //public async Task<IActionResult> RegisterInstructor(RegisterInstructor registerInstructor)
        //{

        //    {

        //        var password = "test@123";
        //        var encrypted = _commanUtility.EncryptPassword(password);
        //        var VerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        //        //Reset token stored to the IAccount Serives 
        //        var ResetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        //        _accountID.RestToken = ResetToken;



        //        var AccountID = Guid.NewGuid();
        //        _accountID.AccountId = AccountID;


        //        var account = new Account
        //        {
        //            VerificationToken = VerificationToken,
        //            FirstName = requestRegister.FName,
        //            LastName = requestRegister.FName,
        //            Email = requestRegister.Email,
        //            PasswordHash = encrypted,
        //            DisplayName = requestRegister.FName + requestRegister.LName,
        //            Address = requestRegister.Address,
        //            Skills = requestRegister.SkillSet,
        //            AccountType = requestRegister.AccountType,



        //        };
        //        var accountId = await _RgService.AddAccount(account);

        //        if (requestRegister.IsStudent)
        //        {
        //            var UserDetails = new UserDetails
        //            {

        //                IsStudent = requestRegister.IsStudent,

        //                Email = requestRegister.Email,
        //                FName = requestRegister.FName,
        //                MName = requestRegister.MName,
        //                LName = requestRegister.LName,
        //                Address = requestRegister.Address,
        //                City = requestRegister.City,
        //                State = requestRegister.State,
        //                Country = requestRegister.Country,
        //                ContactNo = requestRegister.ContactNo,
        //                Education = requestRegister.Education,
        //                SkillSet = requestRegister.SkillSet,
        //                BirthDate = requestRegister.BirthDate,
        //                JoiningDate = requestRegister.JoiningDate,


        //            };

        //            var UD = await _userDetail.AddUserDetail(UserDetails);
        //        }


        //        var StudEnrolment = new StudentEnrollment
        //        {
        //            CategoryCode = requestRegister.CategoryCode,
        //            CourseCode = requestRegister.CourseCode,
        //            CourseFees = requestRegister.CourseFees,
        //            Discount = requestRegister.Discount,
        //            TotalFees = requestRegister.CourseFees - requestRegister.Discount,


        //        };
        //        var StudR = await _studentEnrollment.Enrollment(StudEnrolment);

        //        if (requestRegister.IsPaid)
        //        {
        //            var bill = new BillPayment
        //            {
        //                Amount = requestRegister.PaidAmount,
        //                CourseCode = requestRegister.CourseCode,
        //                IsPaid = requestRegister.IsPaid,


        //            };

        //            var Billpayment = await _BillPayment.BillPayment(bill);

        //        }



        //        try
        //        {

        //            var emailExists = requestRegister.Email;

        //            if (emailExists != null)
        //            {
        //                var senderEmail = requestRegister.Email;
        //                var subject = "Addmission Confrimation ";

        //                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "EmailTemplate", "StudentAdmission.html");



        //                var message = await System.IO.File.ReadAllTextAsync(path);


        //                var resetPasswordLink = _configuration.GetValue<string>("ResetPasswordLink");


        //                var CourseCode = new StudentEnrollment
        //                {
        //                    CourseCode = requestRegister.CourseCode,

        //                };
        //                var CourseName = await _studentEnrollment.GetCourse(CourseCode);

        //                message = message.Replace("{ResetPasswordLink}", resetPasswordLink + "/resetpassword?RestToken=" + ResetToken)
        //                                 .Replace("[Course Name]", CourseName)
        //                                 .Replace("[Address]", requestRegister.Address)
        //                                 .Replace("[Student Name]", requestRegister.FName + requestRegister.LName);

        //                bool isEmailSent = SendEmail.EmailSend(senderEmail, subject, message, null);


        //                if (isEmailSent)
        //                {
        //                    return Ok(true);
        //                }
        //                else
        //                {
        //                    return BadRequest(false);
        //                }

        //            }

        //            return BadRequest(false);
        //        }
        //        catch (Exception ex)
        //        {

        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }

        //    }

        //}







    }
}
