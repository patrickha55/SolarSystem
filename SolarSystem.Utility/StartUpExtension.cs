using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarSystem.Data;
using SolarSystem.Data.Entities;

namespace SolarSystem.Utility
{
    public static class StartUpExtension
    {
        /// <summary>
        /// Name of the policy use for Cors policy.
        /// </summary>
        public const string PolicyName = "AllowAll";

        /// <summary>
        /// Configre cors for application
        /// </summary>
        /// <param name="services"></param>
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

        /// <summary>
        /// Configure application context for EF core.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureApplicationContext(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        /// <summary>
        /// Configure identity framework.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(o =>
            {
                o.Password.RequireUppercase = false;
                o.Password.RequiredUniqueChars = 0;
                o.Password.RequireNonAlphanumeric = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
        }
    }
}
