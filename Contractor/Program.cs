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
using Data.Repositories;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using NLog.Web;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);


    builder.WebHost.UseSentry(o =>
    {
        o.Dsn = "https://115be97abab45146f0236f19396d305b@o4508918186967040.ingest.us.sentry.io/4508918199746560";
        o.Debug = true; // فعال‌سازی لاگ‌های دیباگ
        o.TracesSampleRate = 1.0; // ثبت تمامی تراکنش‌ها
    });

    builder.Logging.ClearProviders().SetMinimumLevel(LogLevel.Trace).AddNLog("nlog.config");


    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    builder.Host.UseNLog();

    builder.Services.AddElmah<SqlErrorLog>(option =>
    {
        option.Path = "/elmah-error";
        option.ConnectionString = builder.Configuration.GetConnectionString("Elmah");
    });

    var app = builder.Build();

    // Exception Handling
    app.UseCustomExceptionHandler();

    if (args.Contains("migrate"))
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Database Migration Applied Successfully.");
        return;
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseElmah();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    // فعال‌سازی Sentry Middleware
    app.UseSentryTracing();



    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
