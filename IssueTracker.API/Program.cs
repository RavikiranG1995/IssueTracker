using IssueTracker.Domain.Repositories;
using IssueTracker.Domain.Services;
using IssueTracker.Infrastructure.Database.Employee;
using IssueTracker.Infrastructure.Database.Helpers;
using IssueTracker.Infrastructure.Database.Image;
using IssueTracker.Infrastructure.Database.Issue;
using IssueTracker.Service.Employee;
using IssueTracker.Service.Image;
using IssueTracker.Service.Issues;
using IssueTracker.Service.StorageService;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IInAppStorageService, InAppStorageService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IIssueRepository, IssueRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

//database
builder.Services.AddSingleton<IDataBaseWrapper>(new DataBaseWrapper(builder.Configuration.GetConnectionString("DefaultConnection")));

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

//app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "IssueFiles")),
    RequestPath = "/IssueFiles"
});

app.UseAuthorization();

app.MapControllers();

app.Run();
