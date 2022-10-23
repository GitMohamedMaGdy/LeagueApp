using AutoMapper;
using LeagueApp.Domain.Models;
using LeagueApp.Domain.Shared;
using LeagueApp.Domain.Shared.IManagers;
using LeagueApp.Domain.Shared.IRepositories;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace LeagueApp.Infrastructure.Shared.Managers
{
    public class PlayerManager : BaseManager<Player>, IPlayerManager
    {
        public PlayerManager(IUnitOfWork unitOfWork, IMapper mapper, IPlayerRepository repository) : base(unitOfWork,mapper, repository)
        {
        }
    }
}
