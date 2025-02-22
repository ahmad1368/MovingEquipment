

using Autofac.Core;
using Common.Utilities;
using Contractor;
using Data;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using WebFramework.Configuration;
using WebFramework.Middlewares;

var builder = WebApplication.CreateBuilder(args);

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//builder.Services.AddDbContext<ApplicationDbContext>(option =>
//{
//    option.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
//    //option.UseSqlServer(Configuration.GetConnectionString("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=MovingEquipment;Data Source=."));
//});
var connectionString = builder.Configuration.GetConnectionString("SqlServer");

// ثبت DbContext در DI
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); // برای SQL Server
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

////////////////////////////////////////
app.IntializeDatabase();

app.UseCustomExceptionHandler();



app.UseHttpsRedirection();

//app.UseElmah(_siteSetting);



app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
////////////////////////////////////////

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();

