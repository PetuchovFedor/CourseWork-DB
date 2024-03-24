using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ELibrary.src.ELibrary.Api.Services;
using ELibrary.src.ELibrary.Api.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace ELibrary.src.ELibrary.Api.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("change-date")]
        public async Task<IActionResult> ChangeDates()
        {
            await _userService.ChangeDate();
            return Ok();
        }
        [HttpGet]
        [Route("get-user/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var dto = await _userService.GetUserById(userId);
                return Ok(dto);
            }
            catch(NullReferenceException)
            {
                return NotFound(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-image/{userId}")]
        public async Task<IActionResult> GetUserImage(int userId)
        {
            try
            {
                var photo = await _userService.GetUserImage(userId);
                return Ok(photo);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("find-user/{name}")]
        public async Task<IActionResult> FindByName(string name, int scipped)
        {
            try
            {
                var result = await _userService.GetUserByName(name, scipped);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Authorize]
        [Route("update-about")]
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto dto)
        {
            try
            {
                await _userService.UpdateAbout(dto);
                return Ok("Success update");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Authorize]
        [Route("update-image")]
        public async Task<IActionResult> UpdatePhoto([FromForm] ChangeFileDto dto)
        {
            try
            {
                await _userService.UpdatePhoto(dto);
                return Ok("Update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut, Authorize]
        [Route("update-password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto dto)
        {
            try
            {
                await _userService.UpdatePassword(dto);
                return Ok("Update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet, Authorize]
        [Route("lks/{userId}")]
        public async Task<IActionResult> GetLksById(int userId)
        {
            var dto = await _userService.GetLksById(userId);
            return Ok(dto);
        }

        [HttpGet]
        [Route("get-authors")]
        public async Task<IActionResult> GetAuthors(int scipped)
        {
            try
            {
                var dto = await _userService.GetAuthors(scipped);
                return Ok(dto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpDelete, Authorize]
        [Route("delete/{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            IActionResult result = Ok();
            try
            {
                await _userService.DeleteUserById(userId);
                return Ok();
            }
            catch(Exception e)
            {
                result = BadRequest(e.Message);
            }
            return result;
        }

        [HttpPut, Authorize]
        [Route("edit-lks")]
        public async Task<IActionResult> EditLks(EditLksDto editLks)
        {
            try
            {
                await _userService.EditLks(editLks);
                return Ok("Edit Success");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost, Authorize]
        [Route("read-book")]
        public async Task<IActionResult> AddToFavor(ReadBookDto dto)
        {
            try
            {
                await _userService.AddBookToFavorities(dto);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet, Authorize]
        [Route("check-read-book")]
        public async Task<IActionResult> CheckFavor(int userId, int bookId)
        {
            try
            {
                var result = await _userService.ChechFavorites(userId, bookId);
                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete, Authorize]
        [Route("delete-read-book")]
        public async Task<IActionResult> DeleteFromFavorities(int userId, int bookId)
        {
            try
            {
                await _userService.DeleteBookFromFavorities(userId, bookId);
                //var result = await _userService.ChechFavorites(userId, bookId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
