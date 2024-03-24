using ELibrary.src.ELibrary.Api.Dto;
using ELibrary.src.ELibrary.Domain.GenreModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using System.Collections.Generic;

namespace ELibrary.src.ELibrary.Domain.BookModel
{
    public interface IBookRepository
    {
        Task<Book> GetById(int id);
        Task<SelectionResult> GetByName(string name, int scipped);
        //Task<List<Book>> GetSortedBooks(SortState sortState);
        Task<SelectionResult> GetSelectionBook(int[] genresIds, SortState sortingType,
            int scipped, int authorId, int readerId);
        Task<List<Genre>> GetBooksGenres(int bookId);
        Task<List<User>> GetAuthorsByBookId(int bookId);
        Task<Book> Create(Book book, List<string> authorsNames, List<int> genresIds);
        Task ChangeGenresBook(int bookId, List<int> genresIds);
        void Delete(Book book);
        void Update(Book book);
        Task ChangeDateById(int bookId);
    }
}
