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

            // Seed

            builder.HasData(
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    DOB = DateTime.Parse("29/12/1993H00:00:00"),
                    Email = "user@mail.com",
                    NormalizedEmail = "USER@MAIL.COM",
                    UserName = "user1993",
                    NormalizedUserName = "USER1993",
                    PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8"
                }
            );
        }
    }
}
