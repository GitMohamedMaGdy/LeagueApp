using LeagueApp.API.Utilites;
using LeagueApp.Domain.DTOS;
using LeagueApp.Domain.Models;
using LeagueApp.Domain.Shared;
using LeagueApp.Domain.Shared.IManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;

namespace LeagueApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamManager _teamManager;
        private readonly IConfiguration _configuration;
        public TeamsController(ITeamManager teamManager,IConfiguration configuration)
        {
            _teamManager = teamManager;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _teamManager.GetAllAsync();
            return Ok(teams);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddTeamWithPlayers(AddTeamDto teamDto)
        {
            var teams = await _teamManager.HandleAddTeamWithPlayers(teamDto);
            return Ok(teams);
        }



        [HttpPut]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Put(AddTeamDto teamDto)
        {
            await _teamManager.UpdateTeam(teamDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeamWithPlayers(int id)
        {
            var team = (await _teamManager.GetAsync(c => c.Id == id));
            if (team == null)
                return BadRequest();
            var result = await _teamManager.DeleteTeam(team);
            return Ok(result);
        }


        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var NotValid = !string.IsNullOrEmpty(file.ContentType) && file.ContentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
            if( NotValid || file.Length <= 0) 
            {
                ModelState.AddModelError("File", $"The request couldn't be processed (Error 1).");
                return BadRequest(ModelState);
            }
            if (UploadFileHelper.CheckFileExtension(file.FileName))
            {
                ModelState.AddModelError("File", $"Invalid image.");
                return BadRequest(ModelState);
            }
            var AppFilesPath = _configuration.GetValue<string>("AppSetting:AppFilesPath");
            var imagePath =  UploadFileHelper.GetAppFilesPath(AppFilesPath, "Assets\\");
            var newFileName =  UploadFileHelper.UploadFile(file, imagePath);
            var id = await _teamManager.AddTeamImage(newFileName);
            return Ok(new { ImageId = id });

        }
    }
}
