using ELibrary.src.ELibrary.Api.Dto;
using ELibrary.src.ELibrary.Domain.RatingModel;
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

    public class RatingController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RatingController(IRatingRepository ratingRepository, IUnitOfWork unitOfWork) 
        { 
            _ratingRepository = ratingRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPut]
        [Route("update-rating"), Authorize]
        public async Task<IActionResult> UpdateRating(RatingDto dto)
        {
            try
            {
                var rating = await _ratingRepository.Get(dto.UserId, dto.BookId);
                rating.Mark = dto.Mark;
                _ratingRepository.Edit(rating);
                _unitOfWork.Commit();
                var result = await _ratingRepository.GetAvgRatingBookById(dto.BookId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("add-rating"), Authorize]
        public async Task<IActionResult> AddRating(RatingDto dto)
        {
            try
            {
                var rating = new Rating(dto.Mark, dto.UserId, dto.BookId);
                await _ratingRepository.Add(rating);
                _unitOfWork.Commit();
                var result = await _ratingRepository.GetAvgRatingBookById(dto.BookId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("delete-rating"), Authorize]
        public async Task<IActionResult> Delete(int userId, int bookId)
        {
            try
            {
                var rating = await _ratingRepository.Get(userId, bookId);
                _ratingRepository.Delete(rating);
                _unitOfWork.Commit();
                var result = await _ratingRepository.GetAvgRatingBookById(bookId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-rating")]
        public async Task<IActionResult> GetRating(int userId, int  bookId)
        {
            try
            {
                var rating = await _ratingRepository.Get(userId, bookId);
                if (rating == null)
                {
                    return Ok(-1);
                }
                var dto = new RatingDto(userId, bookId, rating.Mark.Value);               
                return Ok(dto);                
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
