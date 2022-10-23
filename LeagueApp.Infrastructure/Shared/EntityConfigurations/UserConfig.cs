using LeagueApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueApp.Infrastructure.Shared
{
     class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
          
            builder.HasData(
                new User()
                {
                    Id = "1",
                    Email = "magdy@yahoo.com",
                    UserName = "magdy",
                    PasswordHash = "a4ayc/80/OGda4BO/1o/V0etpOqiLx1JwB5S3beHW0s="
                });
        }

       
    }
}
