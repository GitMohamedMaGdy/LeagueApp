using LeagueApp.Domain.DTOS;
using LeagueApp.Domain.IRepositories;
using LeagueApp.Domain.Models;
using LeagueApp.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LeagueApp.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public AuthRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var existingToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _dbContext.RefreshTokens.Add(token);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            return await _dbContext.RefreshTokens.FindAsync(refreshTokenId);

        }

        public async Task<List<RefreshToken>> GetAllRefreshTokens()
        {
            return await _dbContext.RefreshTokens.ToListAsync();
        }

        public Task<RefreshToken> GetRefreshToken(string refreshTokenId, string username)
        {
            return _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Id == refreshTokenId && r.Subject == username);
        }


        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _dbContext.RefreshTokens.FindAsync(refreshTokenId);
            if (refreshToken != null)
            {
                _dbContext.RefreshTokens.Remove(refreshToken);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public string GenerateHashSHA(string input)
        {

            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public async Task<BaseResponse> ValidateUser(LoginUser userData)
        {

            var user = await _userManager.FindByEmailAsync(userData.Email);
            if (user == null)
                return new BaseResponse()
                {
                    ResponseCode = (int)HttpStatusCode.Unauthorized,
                    ResponseMessage = "UnAunthorized"
                };
            else
            {
                string userPassowrdGenerated = string.IsNullOrEmpty(userData.Password) ? userData.Password : GenerateHashSHA(userData.Password);
                var valid = user.PasswordHash == userPassowrdGenerated;
                if (!valid)
                    return new BaseResponse()
                    {
                        ResponseCode = (int)HttpStatusCode.Unauthorized,
                        ResponseMessage = "UnAunthorized"
                    };
                var roles = await _userManager.GetRolesAsync(user);
                return new UserResponse()
                {
                    ResponseCode = (int)HttpStatusCode.OK,
                    ResponseMessage = "success",
                    Roles = (List<string>)roles,
                    User = user

                };
            }



        }
    }
}
