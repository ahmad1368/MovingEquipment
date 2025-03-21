﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Utilities;
using Contractor;
using Data;
using Data.Repositories;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Entites;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Linq;
using WebFramework.Configuration;
using WebFramework.Middlewares;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
   
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.AddServices(); 
    });

    var settingSection = builder.Configuration.GetSection("SiteSettings");
    var siteSetting = settingSection.Get<SiteSettings>();

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
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(typeof(MappingProfile));

  
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
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();
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