using ELibrary.src.ELibrary.Domain.UserModel;

namespace ELibrary.src.ELibrary.Domain.CommentModel
{
    public interface ICommentRepository
    {
        Task<Comment> GetById(int id);
        Task<Comment> Add(Comment comment);
        Task<int> GetCountCommentOfBook(int bookId);
        void Update(Comment comment);
        void Delete(Comment comment);
        Task<List<Comment>> GetPartByBookId(int bookId, int scipped);
        Task<List<User>> GetUsersByBookId(int bookId);
    }
}
