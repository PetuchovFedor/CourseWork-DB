using ELibrary.src.ELibrary.Domain.CommentModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.src.ELibrary.Infrastructure.Data.CommentModel
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ELibraryDbContext _dbContext;

        public CommentRepository(ELibraryDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<Comment> Add(Comment comment)
        {           
            var entity = await _dbContext.Comment.AddAsync(comment);
            return entity.Entity;
            //throw new NotImplementedException();
        }

        public void Delete(Comment comment)
        {
            _dbContext.Comment.Remove(comment);
           // throw new NotImplementedException();
        }

        public async Task<Comment> GetById(int id)
        {
            return await _dbContext.Comment.FirstOrDefaultAsync(c => c.Id == id);
            //throw new NotImplementedException();
        }

        public async Task<int> GetCountCommentOfBook(int bookId)
        {
            return await _dbContext.Comment
                .Where(c => c.BookId == bookId).CountAsync();
            //throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetPartByBookId(int bookId, int scipped)
        {
            return await _dbContext.Comment.Include(c => c.IdBookNavigation)
                .Include(c => c.IdUserNavigation)
                .Where(c => c.BookId == bookId)
                .Skip(scipped).Take(10).ToListAsync();
            //throw new NotImplementedException();
        }

        public async Task<List<User>> GetUsersByBookId(int bookId)
        {
            return await _dbContext.Comment
                .Where(c => c.BookId == bookId)
                .Select(c => c.IdUserNavigation).ToListAsync();
            //throw new NotImplementedException();
        }

        public void Update(Comment comment)
        {
            _dbContext.Update(comment);
            //throw new NotImplementedException();
        }
    }
}
