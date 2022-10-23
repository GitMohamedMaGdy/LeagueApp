using AutoMapper;
using LeagueApp.API.Utilites;
using LeagueApp.Domain.DTOS;
using LeagueApp.Domain.IRepositories;
using LeagueApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace LeagueApp.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtProvider _jwtProvider;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository authRepository, JwtProvider jwtProvider,IMapper mapper)
        {
            _authRepository = authRepository;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("token")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Login([Required, FromForm] LoginUser userData)
        {
            try
            {
                var response = await _authRepository.ValidateUser(userData);
                if (response.ResponseCode == (int)HttpStatusCode.Unauthorized)
                    return Unauthorized(response);

                var SuccessResponse = _mapper.Map<UserResponse>(response is BaseResponse ? response as BaseResponse : response);
                var token = _jwtProvider.GenerateToken(SuccessResponse.User,SuccessResponse.Roles);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }



    }
}
