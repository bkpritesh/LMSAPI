using Data;
using Data.Repositary;
using Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddSingleton<IUserDetail, UserDetailService>();
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
app.Run();
