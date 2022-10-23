using AutoMapper;
using LeagueApp.Domain.DTOS;
using LeagueApp.Domain.Models;


namespace Loyalty.AppWallet.Api.Mapping
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<Team, AddTeamDto>().ReverseMap();

        }
    }
}
