using ELibrary.src.ELibrary.Api.Dto;
using ELibrary.src.ELibrary.Api.Services;
using ELibrary.src.ELibrary.Api.Services.TokenService;
using ELibrary.src.ELibrary.Domain.RoleModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using ELibrary.src.ELibrary.Infrastructure.UoW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml.Linq;

namespace ELibrary.src.ELibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        public TokenController(IUserRepository userContext, ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userContext;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(AuthResponseDto dto)
        {
            string accessToken = dto.AccessToken;
            string refreshToken = dto.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var strId = principal.FindFirst("Id").Value;
            var id = Convert.ToInt32(strId);
            var user = await _userRepository.GetById(id);
            var token = await _userRepository.GetRefreshToken(id);
            if (user is null || token.Token != refreshToken ||
                token.ExpiresAt <= DateTime.Now)
            {
                return BadRequest();
            }
            string role = user.RoleId - 1 == 0 ? "client" : "admin";

            var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name", user.Name),
                    new Claim("Email", user.Email),
                    new Claim("Role", role)
                };
            var newAccessToken = _tokenService.GenerateAccessToken(claims);
            return Ok(new AuthResponseDto(newAccessToken, refreshToken));
        } 
    }
}
