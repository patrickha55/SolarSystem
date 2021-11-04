using Microsoft.AspNetCore.Identity;
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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName).HasColumnName("First Name").IsRequired(false);
            builder.Property(u => u.LastName).HasColumnName("Last Name").IsRequired(false);
            builder.Property(u => u.DOB).HasColumnName("Day of Birth").HasColumnType("datetime2");
            builder.Property(u => u.PhoneNumber).HasColumnName("Phone Number").IsRequired(false);
        }
    }
}
