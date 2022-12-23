using MFC_VoxMe.Core.Dtos.Email;
using MFC_VoxMe.Core.Interfaces;
using MFC_VoxMe.Infrastructure.Data;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator;
using MFC_VoxMe.Infrastructure.GlobalErrorHandling;
using MFC_VoxMe.Infrastructure.HttpMethods;
using MFC_VoxMe.Infrastructure.HttpMethods.Helpers;
using MFC_VoxMe.Infrastructure.Services;
using MFC_VoxMe_API.BusinessLogic.AccessToken;
using MFC_VoxMe_API.BusinessLogic.JimToVoxMe;
using MFC_VoxMe_API.BusinessLogic.VoxMeToJim;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.HttpMethods;
using MFC_VoxMe_API.Services.Jobs;
using MFC_VoxMe_API.Services.Resources;
using MFC_VoxMe_API.Services.Transactions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
options.CustomSchemaIds(type => type.ToString()));


builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<DapperContext>();

builder.Services.AddAutoMapper(typeof(Program).Assembly); //Added for Automapper config
builder.Services.AddScoped<IJobService, JobService>(); //added for DI 
builder.Services.AddScoped<IResourceService, ResourceService>(); //added for DI 
builder.Services.AddScoped<ITransactionService, TransactionService>(); //added for DI 
builder.Services.AddSingleton<IAccessTokenConfig, AccessTokenConfig>(); //added for DI 
builder.Services.AddSingleton<IHttpRequests, HttpRequests>(); //added for DI 
builder.Services.AddSingleton<IJimToVoxmeHelper, JimToVoxmeHelper>(); //added for DI 
builder.Services.AddSingleton<IVoxmeToJimHelper, VoxmeToJimHelper>(); //added for DI 
builder.Services.AddSingleton<IDynamicQueryGenerator, DynamicQueryGenerator>(); //added for DI 
builder.Services.AddSingleton(typeof(IRequestHelpers<>), typeof(RequestHelpers<>)); //added for DI 
builder.Services.AddTransient<IEmailService, EmailService>(); ; //added for DI 

var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

//Added for Logging with Serilog library, to write logs in a file inside server
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.File("Logging/LogFile.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddlewareExtensions>(); //added for global exception handling

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
