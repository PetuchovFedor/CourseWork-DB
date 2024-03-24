namespace ELibrary.src.ELibrary.Domain.RatingModel
{
    public interface IRatingRepository
    {
        Task<Rating> Get(int userId, int bookId, int rating);
        Task<Rating> Get(int userId, int bookId);
        void Edit(Rating rating);
        Task<Rating> Add(Rating rating);
        Task<double> GetAvgRatingBookById(int bookId);
        void Delete(Rating rating);
    }
}
