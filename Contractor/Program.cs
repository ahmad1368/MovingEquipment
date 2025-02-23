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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;


var builder = WebApplication.CreateBuilder(args);

// تنظیم فرهنگ پیش‌فرض
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

// اضافه کردن سرویس‌ها
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ساخت اپلیکیشن
var app = builder.Build();

// راه‌اندازی پایگاه داده
app.IntializeDatabase();

// مدیریت خطاها
app.UseCustomExceptionHandler();

// تنظیمات HTTPS و مسیرها
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// استفاده از Swagger در محیط توسعه
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// تنظیم مسیرها
app.MapControllers();

// اجرای برنامه
app.Run();
