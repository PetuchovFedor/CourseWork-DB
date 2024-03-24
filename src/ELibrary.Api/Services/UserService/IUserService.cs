using ELibrary.src.ELibrary.Api.Dto;

namespace ELibrary.src.ELibrary.Api.Services
{
    public interface IUserService
    {
        //Task<int> RegistrationUser(RegUserDto dto);
        Task<bool> ChechFavorites(int userId, int bookId);
        Task<UserDto> GetUserById(int id);
        Task<string> GetUserImage(int id);
        Task<FindUserDto> GetUserByName(string name, int scipped);
        Task<FindUserDto> GetAuthors(int scipped);
        Task<UserLksDto> GetLksById(int id);
        Task UpdateAbout(UpdateAboutDto dto);
        Task UpdatePhoto(ChangeFileDto dto);
        Task UpdatePassword(UpdatePasswordDto dto);
        Task DeleteUserById(int id);
        Task AddBookToFavorities(ReadBookDto dto);
        Task DeleteBookFromFavorities(int userId, int bookId);
        Task EditLks(EditLksDto dto);
        Task ChangeDate();

    }
}
