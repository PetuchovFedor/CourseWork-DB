using ELibrary.src.ELibrary.Api.Dto;
using ELibrary.src.ELibrary.Api.Services;
using ELibrary.src.ELibrary.Domain.GenreModel;
using ELibrary.src.ELibrary.Infrastructure.UoW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.src.ELibrary.Api.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class GenreController : ControllerBase
    {
        //private readonly IGenreService _genreService;
        private readonly IGenreRepository _genreRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GenreController(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
        {
            _genreRepository = genreRepository;
            _unitOfWork = unitOfWork;
            //_genreRepository = genreRepository;
            //_unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("get-genres")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Genre> genres = await _genreRepository.GetGenreList();
                return Ok(genres.ConvertAll(g => new GenreDto(g.Id, g.Name)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        [Route("get-genre/{genreId}")]
        public async Task<IActionResult> GetById(int genreId)
        {
            try
            {
                var genre = await _genreRepository.GetById(genreId);
                return Ok(new GenreDto(genre.Id, genre.Name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        [HttpPost, Authorize]
        [Route("create-genre")]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreDto dto)
        {
            try
            {
                var genre = new Genre(dto.Name);
                var result = await _genreRepository.Create(genre);
                _unitOfWork.Commit();
                return Ok(new GenreDto(result.Id, result.Name));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete, Authorize]
        [Route("delete-genre/{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                var genre = await _genreRepository.GetById(id);
                _genreRepository.Delete(genre);
                _unitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut, Authorize]
        [Route("change-genre")]
        public async Task<IActionResult> ChangeGenre(GenreDto dto)
        {
            try
            {
                var genre = await _genreRepository.GetById(dto.Id);
                genre.Name = dto.Name;
                _genreRepository.Update(genre);
                _unitOfWork.Commit();
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
