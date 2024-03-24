using ELibrary.src.ELibrary.Domain.RefreshTokenModel;

namespace ELibrary.src.ELibrary.Domain.UserModel
{
    public interface IAuthRepository
    {
        Task<User> Logination(string email, string password);
        Task UpdateRefreshToken(RefreshToken newToken);
        Task Registration(RefreshToken token);
        Task LogOut(int userId);

    }
}
