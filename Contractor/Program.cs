using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common.Utilities;
using Data;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Linq;
using WebFramework.Configuration;
using WebFramework.CustomMapping;
using WebFramework.Middlewares;
using Microsoft.OpenApi.Models;
using WebFramework.Swagger;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCustomApiVersioning();


    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.AddServices();
       
    });

    var settingSection = builder.Configuration.GetSection("SiteSettings");
    var siteSetting = settingSection.Get<SiteSettings>();

    builder.Services.AddHttpContextAccessor();
    builder.Services.Configure<SiteSettings>(settingSection);
    builder.Services.AddCustomIdentity(siteSetting.IdentitySettings);
    builder.Services.AddJwtAuthentication(siteSetting.JwtSettings);

    builder.WebHost.UseSentry(o =>
    {
        o.Dsn = "https://115be97abab45146f0236f19396d305b@o4508918186967040.ingest.us.sentry.io/4508918199746560";
        o.Debug = true;
        o.TracesSampleRate = 1.0;
    });

    builder.Logging.ClearProviders().SetMinimumLevel(LogLevel.Trace).AddNLog("nlog.config");
     
    builder.Services.AddDbContext(builder.Configuration);
    builder.Services.AddMinimalMvc();
    builder.Services.AddElmahCore(builder.Configuration, siteSetting);

    builder.Services.AddRouting();
    builder.Services.AddEndpointsApiExplorer(); 
    builder.Services.AddSwagger();

    builder.Services.InitializeAutoMapper();   

    builder.Host.UseNLog();

    var app = builder.Build();

    // Exception Handling Middleware
    app.UseCustomExceptionHandler();

    if (args.Contains("migrate"))
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Database Migration Applied Successfully.");
        return;
    }

    app.UseRouting();
    app.IntializeDatabase();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseElmah();
    app.UseHsts(app.Environment);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerAndUI();
    }

    app.MapControllers();
    app.UseSentryTracing();

    ServiceProviderHelper.Instance = app.Services;
    
    IdentityHelper.Initialize(app.Services.GetRequiredService<IHttpContextAccessor>());

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
 