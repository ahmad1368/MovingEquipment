﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using WebFramework.Api;

namespace WebFramework.CustomMapping
{
    public static class AutoMapperConfiguration
    {
        public static void InitializeAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddCustomMappingProfile();
            });

            config.CompileMappings(); // بهینه‌سازی AutoMapper
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper); // اضافه کردن به DI

        }

        public static void AddCustomMappingProfile(this IMapperConfigurationExpression config)
        {
            config.AddCustomMappingProfile(Assembly.GetEntryAssembly());
        }

        public static void AddCustomMappingProfile(this IMapperConfigurationExpression config, params Assembly[] assemblies)
        {
            var allTypes = assemblies.SelectMany(a => a.ExportedTypes);

            var list = allTypes.Where(type => type.IsClass && !type.IsAbstract &&
                type.GetInterfaces().Contains(typeof(IHaveCustomMapping)))
                .Select(type => (IHaveCustomMapping)Activator.CreateInstance(type));

            var profile = new CustomMappingProfile(list);
            config.AddProfile(profile);
        }
    }
}