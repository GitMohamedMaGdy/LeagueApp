using LeagueApp.Domain.Models;
using LeagueApp.Domain.Shared.IRepositories;
using LeagueApp.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueApp.Infrastructure.Shared.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository

    {
        public ImageRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
  
}
