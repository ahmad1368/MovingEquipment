using Autofac.Core;
using Common.Utilities;
using Contractor;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebFramework.Configuration;
using WebFramework.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

// تنظیم فرهنگ پیش‌فرض
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

// ایجاد Builder برای WebApplication
var builder = WebApplication.CreateBuilder(args);

// پیکربندی سرویس‌ها
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ساخت اپلیکیشن
var app = builder.Build();

// راه‌اندازی پایگاه داده هنگام اجرا
if (args.Contains("migrate")) // برای اجرای Migrations از این شرط استفاده کن
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    Console.WriteLine("Database Migration Applied Successfully.");
    return;
}

// مدیریت خطاها
app.UseCustomExceptionHandler();

// پیکربندی HTTPS و مسیرها
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// فعال‌سازی Swagger در محیط توسعه
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ثبت کنترلرها
app.MapControllers();

// اجرای برنامه
app.Run();
