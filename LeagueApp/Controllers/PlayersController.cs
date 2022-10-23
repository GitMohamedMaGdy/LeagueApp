using LeagueApp.Domain.Models;
using LeagueApp.Domain.Shared.IManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerManager _playerManager;
        public PlayersController(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _playerManager.GetAllAsync();
            return Ok(teams);
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(Player player)
        {
            await _playerManager.UpdateWithComplete(player);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayersInTeam(int id)
        {
            var players = await _playerManager.GetManyAsync(c=>c.Id == id);
            return Ok(players);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var player = (await _playerManager.GetManyAsync(c => c.Id == id)).FirstOrDefault();
            if (player == null)
                return NotFound();
            await _playerManager.RemoveWithComplete(player);
            return Ok();
        }
    }
}
