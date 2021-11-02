using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarSystem.Data.Configuration;
using SolarSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RegionConfiguration());
            builder.ApplyConfiguration(new ComponentConfiguration());
            builder.ApplyConfiguration(new BodyConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Body> Bodies { get; set; }
    }
}
