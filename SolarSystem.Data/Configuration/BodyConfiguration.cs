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
        }
    }
}
