using ELibrary.src.ELibrary.Api.Dto;
using ELibrary.src.ELibrary.Domain.BookModel;

namespace ELibrary.src.ELibrary.Api.Services
{
    public interface IBookService
    {
        Task<List<BookIconDto>> GetAll();
        Task<BookDto> GetById(int id);
        Task<BookIconDto> GetBookIconById(int id);
        Task<SelectionResultDto> GetBookIconByName(string name, int scipped);
        Task<string> GetBookFilePath(int id);
        Task<SelectionResultDto> GetSelectionBook(int[] genresIds, SortState sortState, 
            int scipped, int authorId, int readerId);
        Task<int> CreateBook(CreateBookDto book);
        Task EditBook(EditBookDto book);
        Task ChangeBookFile(ChangeFileDto bookFile);
        Task ChangeCover(ChangeFileDto cover);       
        Task DeleteBook(int id);
        Task ChangeDateById(int bookId);
        Task ChangeGenresBook(ChangeGenresBookDto dto);        
    }
}
