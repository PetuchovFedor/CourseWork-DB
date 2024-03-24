using ELibrary.src.ELibrary.Api.Dto;
using ELibrary.src.ELibrary.Api.Services.ImageService;
using ELibrary.src.ELibrary.Domain.CommentModel;
using ELibrary.src.ELibrary.Infrastructure.Data.CommentModel;
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
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageSerice;
        public CommentController(ICommentRepository commentRepository, 
            IUnitOfWork unitOfWork, IImageService imageService)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _imageSerice = imageService;
        }

        [HttpPost, Authorize]
        [Route("create-comment")]
        public async Task<IActionResult> Create(CreateCommentDto dto)
        {
            try
            {
                Comment comment = new(dto.Content, DateTime.Now, dto.UserId,
                    dto.BookId);
                await _commentRepository.Add(comment);
                _unitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Authorize]
        [Route("update-comment")]
        public async Task<IActionResult> Update(UpdateCommentDto dto)
        {
            try
            {
                var comment = await _commentRepository.GetById(dto.CommentId);
                comment.Content = dto.Content;
                _commentRepository.Update(comment);
                _unitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete, Authorize]
        [Route("delete-comment/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var coment = await _commentRepository.GetById(id);
                _commentRepository.Delete(coment);
                _unitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-part-comments/{bookId}")]
        public async Task<IActionResult> GetListComments(int bookId, int scipped)
        {
            try
            {
                var comments = await _commentRepository.GetPartByBookId(bookId, scipped);
                if (comments == null)
                {
                    return Ok("comments over");
                }
                var commentsAsync = comments.ConvertAll(async c => new CommentDto(c.Id, c.UserId, c.BookId,
                    new UserIconDto(c.UserId, c.IdUserNavigation.Name,
                    await _imageSerice.GetImage(c.IdUserNavigation.PhotoUser)), c.Content));
                var result = await Task.WhenAll(commentsAsync);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-count-by-book-id/{bookId}")]
        public async Task<IActionResult> GetCountCommentOfBook(int bookId)
        {
            try
            {
                var result = await _commentRepository.GetCountCommentOfBook(bookId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
