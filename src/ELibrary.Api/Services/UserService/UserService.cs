using ELibrary.src.ELibrary.Api.Dto;
using ELibrary.src.ELibrary.Api.Hashing;
using ELibrary.src.ELibrary.Api.Services.ImageService;
using ELibrary.src.ELibrary.Domain.CommentModel;
using ELibrary.src.ELibrary.Domain.GenreModel;
using ELibrary.src.ELibrary.Domain.RatingModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using ELibrary.src.ELibrary.Infrastructure.UoW;

namespace ELibrary.src.ELibrary.Api.Services
{
    public class UserService : IUserService
    { 
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork,
            IImageService imageService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _imageService = imageService;;
        }

        public async Task AddBookToFavorities(ReadBookDto dto)
        {
            await _userRepository.AddBookToFavorities(dto.UserId, dto.BookId);
            _unitOfWork.Commit();
        }
        public async Task DeleteUserById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) 
            {
                throw new Exception();
            }
            if (user.PhotoUser != _imageService.GetDefaultAvatar())
            {
                _imageService.DeleteImage(user.PhotoUser);
            }
            _userRepository.Delete(user);
            _unitOfWork.Commit();
        }

        public async Task EditLks(EditLksDto dto)
        {
            var user = await _userRepository.GetById(dto.Id);
            user.Name = dto.Name;
            user.Email = dto.Email;
            _userRepository.Update(user);
            _unitOfWork.Commit();
            //throw new NotImplementedException();
        }
        public async Task<UserLksDto> GetLksById(int id)
        {
            var user = await _userRepository.GetById(id);
            var photoPath = await _imageService.GetImage(user.PhotoUser);
            return new UserLksDto(user.Id, user.Name, user.AboutUser,
               photoPath, user.Email, user.DateRegistration);
           //throw new NotImplementedException();
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new NullReferenceException();
            }
            return new UserDto(user.Id, user.Name, user.Email, user.AboutUser,
                await _imageService.GetImage(user.PhotoUser), user.DateRegistration);            
        }

        public async Task<string> GetUserImage(int id)
        {
            var user = await _userRepository.GetById(id);
            var base64Imv = await _imageService.GetImage(user.PhotoUser);
            return base64Imv;
            //throw new NotImplementedException();
        }      

        public async Task UpdateAbout(UpdateAboutDto dto)
        {
            var user = await _userRepository.GetById(dto.Id);
            if (user == null)
            {
                throw new Exception();
            }
            user.AboutUser = dto.About;
            _userRepository.Update(user);
            _unitOfWork.Commit();
            //throw new NotImplementedException();
        }

        public async Task UpdatePassword(UpdatePasswordDto dto)
        {
            var user = await _userRepository.GetById(dto.Id);
            if (user.Password != 
                Hashing.Hashing.Hash(dto.OldPassword)) 
            {
                throw new Exception("Неправильный пароль");
            }
            user.Password = 
                Hashing.Hashing.Hash(dto.NewPassword);
            _userRepository.Update(user);
            _unitOfWork.Commit();
            //throw new NotImplementedException();
        }

        public async Task UpdatePhoto(ChangeFileDto dto)
        {
            var user = await _userRepository.GetById(dto.Id);
            if (user.PhotoUser != _imageService.GetDefaultAvatar())
            {
                _imageService.DeleteImage(user.PhotoUser);
            }
            var fileName = await _imageService.AddImage(dto.File);
            user.PhotoUser = fileName;
            _userRepository.Update(user);
            _unitOfWork.Commit();
            //throw new NotImplementedException();
        }

        public async Task<FindUserDto> GetUserByName(string name, int scipped)
        {
            var findResult = await _userRepository.GetByName(name, scipped);
            var listTask = findResult.Users.ConvertAll(async u =>
                new UserIconDto(u.Id, u.Name, await _imageService.
                GetImage(u.PhotoUser)));
            var users = await Task.WhenAll(listTask);
            return new FindUserDto(users.ToList(), findResult.TotalNumber);
            //throw new NotImplementedException();
        }

        public async Task<FindUserDto> GetAuthors(int scipped)
        {
            var findResult = await _userRepository.GetAuthors(scipped);
            var usersTask = findResult.Users.ConvertAll(async u =>            
                new UserIconDto(u.Id, u.Name, await _imageService
                    .GetImage(u.PhotoUser)));
            var users = await Task.WhenAll(usersTask);
            return new FindUserDto(users.ToList(), findResult.TotalNumber);
            //throw new NotImplementedException();
        }

        public async Task<bool> ChechFavorites(int userId, int bookId)
        {
            return await _userRepository.CheckFavorites(userId, bookId);
            //throw new NotImplementedException();
        }

        public async Task DeleteBookFromFavorities(int userId, int bookId)
        {
            await _userRepository.DeleteBookFromFavorities(userId, bookId);
            _unitOfWork.Commit();
            //throw new NotImplementedException();
        }

        public async Task ChangeDate()
        {
            await _userRepository.ChangeDate();
            _unitOfWork.Commit();
            //throw new NotImplementedException();
        }
    }
}
