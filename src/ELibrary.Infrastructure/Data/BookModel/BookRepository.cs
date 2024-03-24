using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.BooksGenresModel;
using ELibrary.src.ELibrary.Domain.GenreModel;
using ELibrary.src.ELibrary.Domain.RatingModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using ELibrary.src.ELibrary.Domain.UserWriteBookModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ELibrary.src.ELibrary.Infrastructure.Data.BookModel
{
    public class BookRepository: IBookRepository
    {
        private readonly ELibraryDbContext _dbContext;

        public BookRepository(ELibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Book> Create(Book book, List<string> authorsNames
            , List<int> genresIds)
        {
            var authors = new List<User>();
            foreach (string name in authorsNames)
            {
                 authors.Add(await _dbContext.User
                     .SingleOrDefaultAsync(u => u.Name == name));
                    //.Where(u => u.Name == name)) ;
                    //.Select(u => u.Id));
            }
            //var authorsIds = _dbContext.User.Where(u => u.Name == )
            var entity = await _dbContext.Book.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            await _dbContext.WrittenBook.AddRangeAsync(authors
                .ConvertAll(a => new UserWriteBook(a.Id, entity.Entity.Id,
                entity.Entity, a)));
            await _dbContext.BooksGenres.AddRangeAsync(genresIds
                .ConvertAll(gId => new BooksGenres(entity.Entity.Id, gId)));
            return entity.Entity;
        }

        public async Task<Book> GetById(int id)
        {
            return await _dbContext.Book.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async void Update(Book book)
        {
            _dbContext.Book.Update(book);
        }

        public void Delete(Book book)
        {
             _dbContext.Book.Remove(book);
            //_dbContext.Book.Remove(x => x.Id == id);
            //throw new NotImplementedException();
        }

        public async Task<List<Genre>> GetBooksGenres(int bookId)
        {
            var genres = await _dbContext.BooksGenres
                .Where(b => b.BookId == bookId)
                .Select(b => b.IdGenreNavigation).ToListAsync();
            return genres;
        }

        public async Task<List<User>> GetAuthorsByBookId(int bookId)
        {
            var authors = await _dbContext.WrittenBook
                .Where(b => b.BookId == bookId)
                .Select(b => b.IdUserNavigation).ToListAsync();
            return authors;
            //throw new NotImplementedException();
        }
        public async Task<SelectionResult> GetSelectionBook(int[] genresIds, 
            SortState sortingType, int scipped, int authorId, int readerId)
        {
            var books = new List<Book>();
            if (authorId > 0)
            {
                books = await _dbContext.WrittenBook
                    .Where(b => b.UserId == authorId)
                    .Select(b => b.IdBookNavigation).Distinct()
                    .ToListAsync();
            }
            else if (readerId > 0)
            {
                books = await _dbContext.ReadBooks
                    .Where(b => b.UserId == readerId)
                    .Select(b => b.IdBookNavigation).Distinct()
                    .ToListAsync();
            }
            else
            {
                books = await _dbContext.Book.ToListAsync();
            }
            if (genresIds.Length != 0)
            {
                // Отфильтровать список книг по массиву идентификаторов жанров
                books = books.Where(book =>
                    genresIds.All(genreId =>
                        _dbContext.BooksGenres.Any(bg => bg.BookId == book.Id && bg.GenreId == genreId)
                    )
                ).ToList();
                //books = books.Where(b => _dbContext.BooksGenres
                //.Any(bg => bg.BookId == b.Id && genresIds.Contains(bg.GenreId))).ToList();
            }          
            //.Skip(scipped).Take(50).ToList();
            int totalNumber = books.Count;
            books = books.Skip(scipped).Take(15).ToList();
            //var books = await _dbContext.BooksGenres
            //    //.Include(b => b.IdBookNavigation)
            //    .Where(b => genresIds.Contains(b.GenreId))
            //    .Select(b => b.IdBookNavigation)
            //    .Skip(scipped).Take(50)
            //    .ToListAsync();
            //var books = _dbContext.B;

            var result = new List<Book>();
            switch (sortingType)
            {
                case SortState.None:
                    {
                        result = books; 
                        break;
                    }
                case SortState.DateDesc:
                    {
                        result = books.OrderByDescending(b => b.PublicationDate).ToList();
                        break;
                    }
                case SortState.DateAsc:
                    {
                        result = books.OrderBy(b => b.PublicationDate).ToList();
                        break;
                    }
                case SortState.NameAsc:
                    {
                        result = books.OrderBy(b => b.Name).ToList();
                        break;
                    }
                case SortState.NameDesc:
                    {
                        result = books.OrderByDescending(b => b.Name).ToList();
                        break;
                    }
                case SortState.Rating:
                    {
                        var averageRatings = books.GroupJoin(
                            _dbContext.Rating,
                            book => book.Id,
                            rating => rating.BookId,
                            (book, ratingGroup) => new {
                                Book = book,
                                AverageRating = ratingGroup.Average(r => r.Mark)
                            });
                        result = averageRatings
                            .OrderByDescending(r => r.AverageRating)
                            .Select(r => r.Book).ToList();
                        break;
                    }
                default:
                    break;
            }
            return new SelectionResult(result, totalNumber);
            //throw new NotImplementedException();
        }

        public async Task<SelectionResult> GetByName(string name, int scipped)
        {
            string pattern = "%" + name + "%";
            var books = await _dbContext.Book.Where(b => EF.Functions
            .Like(b.Name!, pattern)).ToListAsync();
            var number = books.Count();
            books = books.Skip(scipped).Take(10).ToList();
            return new SelectionResult(books, number);
        }

        public async Task ChangeDateById(int bookId)
        {
            var book = await GetById(bookId);
            book.PublicationDate = DateTime.Now.AddDays(-4);
            _dbContext.Book.Update(book);
            //throw new NotImplementedException();
        }

        public async Task ChangeGenresBook(int bookId, List<int> genresIds)
        {
            var newBookGenres = genresIds.ConvertAll(g => new BooksGenres(bookId, g));
            var oldBookGenres = _dbContext.BooksGenres
                .Where(g => g.BookId == bookId);
            _dbContext.BooksGenres.RemoveRange(oldBookGenres);
            await _dbContext.AddRangeAsync(newBookGenres);
            //throw new NotImplementedException();
        }
    }
}
