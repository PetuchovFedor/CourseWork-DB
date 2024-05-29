using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.CommentModel;
using ELibrary.src.ELibrary.Domain.GenreModel;
using ELibrary.src.ELibrary.Domain.RatingModel;
using ELibrary.src.ELibrary.Domain.RefreshTokenModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using ELibrary.src.ELibrary.Domain.UserReadBookModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ELibrary.src.ELibrary.Infrastructure.Data.UserModel
{
    public class UserRepository : IUserRepository
    {
        private readonly ELibraryDbContext _dbContext;

        public UserRepository(ELibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetById(int id)
        {
            return await _dbContext.User.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _dbContext.User.SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> Add(User user)
        {
            EntityEntry<User> entity = await _dbContext.User.AddAsync(user);
            return entity.Entity;
        }

        public void Update(User user)
        {
            _dbContext.User.Update(user);
        }      

        public async Task Delete(User user)
        {
            var userBooks = await _dbContext.WrittenBook
                .Where(x => x.UserId == user.Id)
                .Select(x => x.IdBookNavigation)
                .ToListAsync();
            _dbContext.Book.RemoveRange(userBooks);
            _dbContext.User.Remove(user);            
        }

        public async Task<RefreshToken> GetRefreshToken(int userId)
        {
            var token = await _dbContext.Tokens
                .SingleOrDefaultAsync(t => t.UserId == userId);
            return token;
            //throw new NotImplementedException();
        }

        public async Task AddBookToFavorities(int userId, int bookId)
        {
            var temp = new UserReadBook(userId, bookId);
            await _dbContext.ReadBooks.AddAsync(temp);
            //throw new NotImplementedException();
        }

        public async Task<FindUserResult> GetByName(string name, int scipped)
        {
            string pattern = "%" + name + "%";
            var users =  await _dbContext.User.Where(u => EF.Functions
            .Like(u.Name!, pattern)).ToListAsync();
            var number = users.Count();
            users = users.Skip(scipped).Take(10).ToList();
            return new FindUserResult(users, number);
            //throw new NotImplementedException();
        }

        public async Task<FindUserResult> GetAuthors(int scipped)
        {
            var authors = await _dbContext.WrittenBook
                .Select(wrb => wrb.IdUserNavigation).Distinct()
                .ToListAsync();
            var number = authors.Count();
            authors = authors.Skip(scipped).Take(15).ToList();
            return new FindUserResult (authors, number);
        }

        public async Task<bool> CheckFavorites(int userId, int bookId)
        {
            var user = await _dbContext.ReadBooks
                .SingleOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);
            if (user == null)
            {
                return false;
            }
            return true;
            //throw new NotImplementedException();
        }

        public async Task DeleteBookFromFavorities(int userId, int bookId)
        {
            var readBook = await _dbContext.ReadBooks
                .SingleOrDefaultAsync(x => x.UserId == userId &&
                x.BookId == bookId);
            if (readBook != null) 
            {
                _dbContext.ReadBooks.Remove(readBook);
            }
            //throw new NotImplementedException();
        }

        public async Task ChangeDate()
        {
            var users = await _dbContext.User.ToListAsync();
            users.ForEach(user => user.DateRegistration = DateTime.Now.AddDays(-7));
            _dbContext.User.UpdateRange(users);
            var books = await _dbContext.Book.ToListAsync();
            books.ForEach(book => book.PublicationDate = DateTime.Now.AddDays(-7));
            //throw new NotImplementedException();
        }
    }
}
