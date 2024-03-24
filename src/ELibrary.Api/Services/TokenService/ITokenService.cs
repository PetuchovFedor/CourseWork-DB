using ELibrary.src.ELibrary.Domain.UserModel;
using System.Security.Claims;

namespace ELibrary.src.ELibrary.Api.Services.TokenService
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
