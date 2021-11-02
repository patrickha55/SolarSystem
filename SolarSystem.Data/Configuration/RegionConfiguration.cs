using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarSystem.Data.Entities;

namespace SolarSystem.Data.Configuration
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.ToTable("Regions");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).IsRequired();

            builder.Property(r => r.Name).HasColumnType("nvarchar").HasMaxLength(255).IsRequired();
            builder.Property(r => r.DistanceToTheSun).HasColumnName("Distance To The Sun (AU)").IsRequired();

            builder.Property(b => b.CreatedAt).HasColumnName("Created At").HasColumnType("datetime2").IsRequired();
            builder.Property(b => b.UpdatedAt).HasColumnName("Updated At").HasColumnType("datetime2").IsRequired();
        }
    }
}
