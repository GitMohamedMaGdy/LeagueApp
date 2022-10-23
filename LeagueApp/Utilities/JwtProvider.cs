using LeagueApp.Domain.IRepositories;
using LeagueApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace LeagueApp.API.Utilites
{
    public class JwtProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        public JwtProvider(IConfiguration configuration, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _authRepository = authRepository;
        }

        /// <summary>
        /// Generates Access token for the user.
        /// </summary>
        /// <param name="user">The user that will be granted the access token.</param>
        /// <param name="clientId">The clientId</param>
        /// <returns>The generated Access token.</returns>
        public string GenerateToken(User user, List<string> Roles)
        {
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, Roles.ToString()),
            new Claim("userid", user.Id.ToString()),
            new Claim("roleid", Roles.ToString()),

            };

            var appKey = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetting:Token").Value);

            var key = new SymmetricSecurityKey(appKey);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration.GetSection("AppSetting:ExpireTimeSpanInMinutes").Value)),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateEncodedJwt(tokenDescriptor);
            return token;
        }
    }
}
