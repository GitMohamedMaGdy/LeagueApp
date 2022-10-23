
using LeagueApp.Domain.DTOS;
using LeagueApp.Domain.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeagueApp.Domain.IRepositories
{
    public interface IAuthRepository
    {
        Task<bool> AddRefreshToken(RefreshToken token);
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> FindRefreshToken(string refreshTokenId);
        Task<List<RefreshToken>> GetAllRefreshTokens();
        Task<RefreshToken> GetRefreshToken(string refreshTokenId, string username);
        string GenerateHashSHA(string input);
        Task<BaseResponse> ValidateUser(LoginUser userData);

    }
}
