using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarSystem.Data;

namespace SolarSystem.Utility
{
    public static class StartUpExtension
    {
        public const string PolicyName = "AllowAll";

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy(PolicyName, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        public static void ConfigureApplicationContext(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}
