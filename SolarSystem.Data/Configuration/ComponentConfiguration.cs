using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarSystem.Data.Entities;

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
        }
    }
}
