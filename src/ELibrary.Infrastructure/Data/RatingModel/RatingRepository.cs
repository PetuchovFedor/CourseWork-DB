using ELibrary.src.ELibrary.Domain.RatingModel;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.src.ELibrary.Infrastructure.Data.RatingModel
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ELibraryDbContext _dbContext;

        public RatingRepository(ELibraryDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<Rating> Add(Rating rating)
        {
            var ent = await _dbContext.Rating.AddAsync(rating);
            return ent.Entity;
            //throw new NotImplementedException();
        }

        public void Delete(Rating rating)
        {
            _dbContext.Rating.Remove(rating);
            //throw new NotImplementedException();
        }

        public void Edit(Rating rating)
        {
            _dbContext.Rating.Update(rating);
            //throw new NotImplementedException();
        }

        public async Task<Rating> Get(int userId, int bookId, int rating)
        {
            return await _dbContext.Rating.SingleOrDefaultAsync(
                r => r.Mark == rating && r.BookId == bookId && r.UserId == userId);
            //throw new NotImplementedException();
        }

        public async Task<Rating> Get(int userId, int bookId)
        {
            return await _dbContext.Rating.SingleOrDefaultAsync(
                r => r.BookId == bookId && r.UserId == userId);
            //throw new NotImplementedException();
        }

        public async Task<double> GetAvgRatingBookById(int bookId)
        {
            var result = await _dbContext.Rating
                .Where(r => r.BookId == bookId)
                .AverageAsync(r => r.Mark);

            return result != null ? (double)result : 0.0;
            //throw new NotImplementedException();
        }
    }
}
