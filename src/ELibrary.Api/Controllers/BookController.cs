using ELibrary.src.ELibrary.Api.Dto;
using ELibrary.src.ELibrary.Api.Services;
using ELibrary.src.ELibrary.Domain.BookModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Http;

namespace ELibrary.src.ELibrary.Api.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [Route("change-date/{bookId}")]
        public async Task<IActionResult> ChangeDateById(int bookId)
        {
            await _bookService.ChangeDateById(bookId);
            return Ok();
        }

        [HttpPut, Authorize]
        [Route("change-genres")]
        public async Task<IActionResult>ChangeGenres([FromBody] ChangeGenresBookDto dto)
        {
            try
            {
                await _bookService.ChangeGenresBook(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{bookId}")]
        public async Task<IActionResult> GetById(int bookId)
        {
            try
            {
                var dto = await _bookService.GetById(bookId);
                return Ok(dto);
            }
            catch(NullReferenceException)
            {
                return NotFound(bookId);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        [Route("get-book-file/{bookId}")]
        public async Task<IActionResult> GetBookFile(int bookId)
        {
            try
            {
                var filePath = await _bookService.GetBookFilePath(bookId);
                var book = await _bookService.GetBookIconById(bookId);
                string downloadName = book.Name + ".fb2";
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/octet-stream", downloadName);
                //var filePath = await _bookService.GetBookFilePath(bookId);
                //var book = await _bookService.GetBookIconById(bookId);
                ////string path = "Files/forest.png";
                //byte[] fileContent = await System.IO.File.ReadAllBytesAsync(filePath);  // считываем файл в массив байтов
                //string contentType = "text/xml";       // установка mime-типа
                //string downloadName = book.Name + ".fb2";  // установка загружаемого имени
                
                //return Results.File(fileContent, contentType, downloadName);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
                //return BadRequest(ex.Message);
            }
        }

        //[HttpGet]
        //[Route("get-by-genre/{genreId}")]
        //public async Task<IActionResult> GetBooksByGenreId(int genreId)
        //{
        //    var booksDto = await _bookService.GetBooksIconByGenre(genreId);
        //    return Ok(booksDto);
        //}

        [HttpGet]
        [Route("get-books")]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAll();
            return Ok(books);
        }

        [HttpPost]
        [Route("get-selection-books")]
        public async Task<IActionResult> GetSelectionBooks(
            [FromBody] SelectionBookDto dto)
        {
            try
            {
                var result = await _bookService.GetSelectionBook(dto.GenresIds.ToArray(),
                    (SortState)dto.SortingType, dto.Scipped, 
                    dto.AuthorId, dto.ReaderId);
                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBook([FromForm] CreateBookDto dto)
        {
            try
            {
                var id = await _bookService.CreateBook(dto);
                return Ok(id);
            }
            catch( Exception e )
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpDelete, Authorize]
        [Route("delete/{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            IActionResult result = Ok();
            try
            {
                await _bookService.DeleteBook(bookId);
            }
            catch(Exception ex)
            {
                result = BadRequest(ex.Message);
            }
            return result;
        }

        [HttpPut]
        [Route("edit-book")]
        public async Task<IActionResult> EditBook(EditBookDto dto)
        {
            IActionResult result = Ok();
            try
            {
                await _bookService.EditBook(dto);
            }
            catch(Exception ex)
            {
                result = BadRequest(ex.Message);
            }
            return result;
        }
        [HttpPut]
        [Route("change-cover")]
        public async Task<IActionResult> ChangeCover([FromForm] ChangeFileDto dto)
        {
            IActionResult result = Ok();
            try
            {
                await _bookService.ChangeCover(dto);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }
            return result;
        }

        [HttpPut]
        [Route("change-file")]
        public async Task<IActionResult> ChangeBookFile([FromForm] ChangeFileDto dto)
        {
            IActionResult result = Ok();
            try
            {
                await _bookService.ChangeBookFile(dto);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }
            return result;
        }

        [HttpGet]
        [Route("find-book/{bookName}")]
        public async Task<IActionResult> FindByName(string bookName, int scipped) 
        {
            try
            {
                var result = await _bookService.GetBookIconByName(bookName, scipped);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
