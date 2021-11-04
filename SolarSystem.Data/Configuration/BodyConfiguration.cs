using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem.Data.Configuration
{
    internal class BodyConfiguration : IEntityTypeConfiguration<Body>
    {
        public void Configure(EntityTypeBuilder<Body> builder)
        {
            builder.ToTable("Bodies");

            builder.Property(b => b.Id).IsRequired();

            builder.Property(b => b.Name).HasMaxLength(255).HasColumnType("nvarchar").IsRequired();
            builder.Property(b => b.EarthMass).HasColumnName("Earth Mass (AU)").IsRequired();
            builder.Property(b => b.DistanceToTheSun).HasColumnName("Distance To The Sun (AU)").IsRequired();
            builder.Property(b => b.ComponentId).HasColumnName("Component Id").IsRequired();
            builder.Property(b => b.RegionId).HasColumnName("Region Id").IsRequired();

            builder.Property(b => b.CreatedAt).HasColumnName("Created At").HasColumnType("datetime2").IsRequired();
            builder.Property(b => b.UpdatedAt).HasColumnName("Updated At").HasColumnType("datetime2").IsRequired();

            builder.HasOne(b => b.Component).WithMany(c => c.Bodies).HasForeignKey(b => b.ComponentId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(b => b.Region).WithMany(c => c.Bodies).HasForeignKey(b => b.RegionId).OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Body { Id = 1, Name = "Sun", EarthMass = 332900, DistanceToTheSun = 0, ComponentId = 1, RegionId = 1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Body { Id = 2, Name = "Earth", EarthMass = 1321, DistanceToTheSun = 1, ComponentId = 2, RegionId = 1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Body { Id = 3, Name = "Jupiter", EarthMass = 332900, DistanceToTheSun = 5.2, ComponentId = 3, RegionId = 2, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );
        }
    }
}
