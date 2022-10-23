using LeagueApp.Domain.Models;
using LeagueApp.Domain.Shared;
using LeagueApp.Domain.Shared.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueApp.Infrastructure.Shared.Repositories
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
