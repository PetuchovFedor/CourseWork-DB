using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.CommentModel;
using ELibrary.src.ELibrary.Domain.RefreshTokenModel;

namespace ELibrary.src.ELibrary.Domain.UserModel
{
    public interface IUserRepository
    {
        Task<User> Add(User user);
        Task<User> GetById(int id);
        Task AddBookToFavorities(int userId, int bookId);
        Task DeleteBookFromFavorities(int userId, int bookId);
        Task<RefreshToken> GetRefreshToken(int userId);
        Task<User> GetByEmail(string email);
        Task<bool> CheckFavorites(int userId, int bookId);
        Task<FindUserResult> GetByName(string name, int scipped);
        Task<FindUserResult> GetAuthors(int scipped);
        void Update(User user);
        Task Delete(User user);
        Task ChangeDate();
    }
}
