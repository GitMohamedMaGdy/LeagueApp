using AutoMapper;
using LeagueApp.Domain.DTOS;
using LeagueApp.Domain.Models;
using LeagueApp.Domain.Shared;
using LeagueApp.Domain.Shared.IManagers;
using LeagueApp.Domain.Shared.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeagueApp.Infrastructure.Shared.Managers
{
    public class TeamManager : BaseManager<Team>, ITeamManager
    {

        private readonly IMapper _mapper;
        private readonly IPlayerRepository _playerRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ITeamRepository _teamRepository;
        public TeamManager(IUnitOfWork unitOfWork, IMapper mapper,
              ITeamRepository repository, IPlayerRepository playerRepository, IImageRepository imageRepository, ITeamRepository teamRepository) : base(unitOfWork, mapper, repository)
        {
            _mapper = mapper;
            _playerRepository = playerRepository;
            _imageRepository = imageRepository;
            _teamRepository = teamRepository;
        }

        public async Task<BaseResponse> HandleAddTeamWithPlayers(AddTeamDto teamDto)
        {
            try
            {
                await BeginTransaction();
                var team = _mapper.Map<Team>(teamDto);
                await Add(team);
                foreach (var player in teamDto.Players)
                {
                    await _playerRepository.Add(player);
                }
                await CommitTransaction();
                return new BaseResponse()
                {
                    ResponseCode = 0,
                    ResponseMessage = "Success"
                };
                
            }
            catch(Exception ex)
            {
                await RollbackTransaction();
                return new BaseResponse()
                {
                    ResponseCode = -1,
                    ResponseMessage = "Failed with" + ex.Message
                };
            }
        }
        public async Task<BaseResponse> HandleDeleteTeamWithPlayers(AddTeamDto teamDto)
        {
            try
            {
                await BeginTransaction();
                var team = _mapper.Map<Team>(teamDto);
                await DeleteTeam(team);
                foreach (var player in teamDto.Players)
                {
                    await _playerRepository.Add(player);
                }
                await CommitTransaction();
                return new BaseResponse()
                {
                    ResponseCode = 0,
                    ResponseMessage = "Success"
                };

            }
            catch (Exception ex)
            {
                await RollbackTransaction();
                return new BaseResponse()
                {
                    ResponseCode = -1,
                    ResponseMessage = "Failed with" + ex.Message
                };
            }
        }
        public async Task<BaseResponse> DeleteTeam(Team team)
        {
            try
            {
                await BeginTransaction();
                var playersInTeam = await _playerRepository.GetAllAsync(c => c.TeamId == team.Id);
                foreach (var player in playersInTeam)
                {
                    player.TeamId = 0;
                    await _playerRepository.Update(player);
                }
                await Remove(team);
                await CommitTransaction();
                return new BaseResponse()
                {
                    ResponseCode = 0,
                    ResponseMessage = "Success"
                };
            }
            catch(Exception ex)
            {
                await RollbackTransaction();
                return new BaseResponse()
                {
                    ResponseCode = -1,
                    ResponseMessage = "Failed with" + ex.Message
                };
            }
            
        }

        public async Task<BaseResponse> UpdateTeam(AddTeamDto teamDto)
        {
            try
            {
                await BeginTransaction();
                var team = _mapper.Map<Team>(teamDto);
                var players = await _playerRepository.GetAllAsync(c => c.TeamId == teamDto.Id);
                foreach (var player in teamDto.Players)
                {
                    await _playerRepository.Update(player);
                }
                await _teamRepository.Update(team);
                await CommitTransaction();
                return new BaseResponse()
                {
                    ResponseCode = 0,
                    ResponseMessage = "Success"
                };
            }
            catch (Exception ex)
            {
                await RollbackTransaction();
                return new BaseResponse()
                {
                    ResponseCode = -1,
                    ResponseMessage = "Failed with" + ex.Message
                };
            }

        }

        public async Task<int> AddTeamImage(string newFileName)
        {
            var image = new Image() { ImageName = newFileName };
            await _imageRepository.Add(image);
            await Complete();
            return image.Id;
        }
    }
}
