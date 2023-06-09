using Data;
using Data.Repositary;
using Data.Services;
using LMS.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model;
using System.Text;





var builder = WebApplication.CreateBuilder(args);






builder.Services.AddScoped<IDashBoard, DashBoardService>();
builder.Services.AddScoped<AssistmentService>();
builder.Services.AddScoped<IAssistment, AssistmentService>();
builder.Services.AddScoped<IInquiries,InquiriesService>();
builder.Services.AddScoped<BatchDetailService>(); 
builder.Services.AddScoped<IBatchDetail, BatchDetailService>();
builder.Services.AddScoped<BatchService>();
builder.Services.AddSingleton<IBatch,BatchService>();
builder.Services.AddSingleton<IBIllPayment, BillPaymentSevice>();
builder.Services.AddSingleton<IBIllPayment, BillPaymentSevice>();
builder.Services.AddSingleton<IStudentEnrollment, StudentEnrollmentService>();
builder.Services.AddSingleton<ICommanUtility,CommanUtility>();
builder.Services.AddSingleton<IAccountID, AccountIDService>();
builder.Services.AddSingleton<IFilterCourse, FilterCourseService>();
builder.Services.AddScoped<IUserDetail, UserDetailService>();
builder.Services.AddSingleton<IStateandCities, StateandCitiesServices>();
builder.Services.AddSingleton<ICourse,CourseService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IValidateResetToken, ValidateResetTokenService>();
builder.Services.AddScoped<IResetPassword,ResetPasswordService>();
builder.Services.AddScoped<IValidateVerificationToken,ValidateVerificationTokenService>();
builder.Services.AddScoped<ValidateVerificationTokenService>();
builder.Services.AddScoped<IEducation,EducationService>();  
builder.Services.AddScoped<IValidateVerificationToken, ValidateVerificationTokenService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();    
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
builder.Services.AddScoped<IDocument, DocumentService>();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = "GyanShaktiTech.com",
        ValidIssuer = "GyanShaktiTech",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GyanShaktiJWTKey"))
    };
});

builder.Services.AddSwaggerGen(option =>
{
	option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[]{}
		}
	});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("corsapp");

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Document")),
    RequestPath = new PathString("/Document")
});

app.Run();
