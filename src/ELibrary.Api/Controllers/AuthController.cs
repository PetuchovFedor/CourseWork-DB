using ELibrary.src.ELibrary.Api.Dto;
//using ELibrary.src.ELibrary.Api.Services.AuthService;
using ELibrary.src.ELibrary.Api.Services.ImageService;
using ELibrary.src.ELibrary.Api.Services.TokenService;
using ELibrary.src.ELibrary.Domain.RefreshTokenModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using ELibrary.src.ELibrary.Infrastructure;
using ELibrary.src.ELibrary.Infrastructure.UoW;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace ELibrary.src.ELibrary.Api.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class AuthController : ControllerBase
    {
        //private readonly IHttpContextAccessor _context;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthRepository _authRepository;
        private readonly string _defaultAvatar;
        public AuthController(IUnitOfWork unitOfWork, IAuthRepository authRepository, 
            ITokenService tokenService, IImageService imageService,
            IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _authRepository = authRepository;
            _tokenService = tokenService;
            _defaultAvatar = imageService.GetDefaultAvatar();
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            //IActionResult result = Ok();
            try
            {
                var user = await _authRepository.Logination(dto.Email, dto.Password);
                string role = user.RoleId - 1 == 0 ? "client" : "admin";
                //var claims = new List<Claim>
                //{
                //    new Claim("Id", user.Id.ToString()),
                //    new Claim(ClaimTypes.Name, user.Name),
                //    new Claim("Email", user.Email),
                //    new Claim(ClaimTypes.Role, role)
                //};
                var identity = GetIdentity(user.Id, user.Name, user.Email, role);

                var accessToken = _tokenService.GenerateAccessToken(identity.Claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                RefreshToken token = new(user.Id, refreshToken, DateTime.Now,
                    DateTime.Now.AddDays(7));
                await _authRepository.UpdateRefreshToken(token);
                _unitOfWork.Commit();
                //string role = user.RoleId - 1 == 0 ? "client" : "admin";
                //var claims = new List<Claim>
                //{
                //    new Claim("Id", user.Id.ToString()),
                //    new Claim(ClaimTypes.Name, user.Name),
                //    new Claim("Email", user.Email),
                // new Claim(ClaimTypes.Role, role)
                //};
                //var response = await _authService.Login(dto);
                return Ok(new AuthResponseDto(accessToken, refreshToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration([FromBody] RegUserDto dto)
        {
            try
            {
                var user = new User(dto.Name, 1, dto.Email,
                Hashing.Hashing.Hash(dto.Password), null,
                DateTime.Now, _defaultAvatar);
                var tempUser = await _userRepository.GetByEmail(user.Email);
                if (tempUser != null)
                {
                    throw new Exception("User with this email already exist");
                }
                user = await _userRepository.Add(user);
                _unitOfWork.Commit();
                string role = user.RoleId - 1 == 0 ? "client" : "admin";
                var identity = GetIdentity(user.Id, user.Name, user.Email, role);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                //await HttpContext.SignInAsync(claimsPrincipal);
                //var claims = new List<Claim>
                //{
                //    new Claim("Id", user.Id.ToString()),
                //    new Claim(ClaimTypes.Name, user.Name),
                //    new Claim("Email", user.Email),
                //    new Claim(ClaimTypes.Role, role)
                //};
                var accessToken = _tokenService.GenerateAccessToken(identity.Claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                RefreshToken token = new RefreshToken(user.Id, refreshToken, DateTime.Now,
                    DateTime.Now.AddDays(7));
                await _authRepository.Registration(token);
                var allClaims = HttpContext.User.Claims.Select(c => $"{c.Type}: {c.Value}");
                Console.WriteLine("All Claims: " + string.Join(", ", allClaims));
                //await _authRepository.UpdateRefreshToken(token);
                _unitOfWork.Commit();
                return Ok(new AuthResponseDto(accessToken, refreshToken));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("logout/{userId}")]
        public async Task<IActionResult> Logout(int userId)
        {
            await _authRepository.LogOut(userId);
            _unitOfWork.Commit();
            return Ok();
        }
        private ClaimsIdentity GetIdentity(int id, string name, string email,
            string role)
        {
            var claims = new List<Claim>
                {
                    new Claim("Id", id.ToString()),
                    new Claim("Name", name),
                    new Claim("Email", email),
                    new Claim("Role", role)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token");
            return claimsIdentity;
        }
    }
    

}
