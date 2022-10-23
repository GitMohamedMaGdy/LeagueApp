

using LeagueApp.Domain.DTOS;
using LeagueApp.Domain.Models;
using System.Threading.Tasks;

namespace LeagueApp.Domain.Shared.IManagers
{
    public interface ITeamManager : IBaseManager<Team>
    {
       Task<BaseResponse> HandleAddTeamWithPlayers(AddTeamDto teamDto);
       Task<BaseResponse> DeleteTeam(Team team);
       Task<int> AddTeamImage(string newFileName);
       Task<BaseResponse> HandleDeleteTeamWithPlayers(AddTeamDto teamDto);
        Task<BaseResponse> UpdateTeam(AddTeamDto team);


    }
}
