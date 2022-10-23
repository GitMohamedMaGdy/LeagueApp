using LeagueApp.Domain.Models;
using LeagueApp.Domain.Shared;
using LeagueApp.Domain.Shared.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueApp.Infrastructure.Shared.Repositories
{
    public class TeamRepository : Repository<Team>, ITeamRepository

    {
        public TeamRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
