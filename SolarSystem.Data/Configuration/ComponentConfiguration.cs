using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarSystem.Data.Entities;
using System;

namespace SolarSystem.Data.Configuration
{
    public class ComponentConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> builder)
        {
            builder.ToTable("Components");

            builder.Property(c => c.Id).IsRequired();

            builder.Property(c => c.Name).HasMaxLength(255).IsRequired();
            builder.Property(c => c.Type).HasMaxLength(100).IsRequired();

            builder.Property(b => b.CreatedAt).HasColumnName("Created At").HasColumnType("datetime2").IsRequired();
            builder.Property(b => b.UpdatedAt).HasColumnName("Updated At").HasColumnType("datetime2").IsRequired();

            builder.HasData(
                new Component { Id = 1, Name = "Star", Type = "G2 main-sequence star", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Component { Id = 2, Name = "Rocky Planet", Type = "Rocky Planet", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Component { Id = 3, Name = "Gas Planet", Type = "Gas Planet", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );
        }
    }
}
