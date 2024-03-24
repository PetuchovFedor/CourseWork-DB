using ELibrary.src.ELibrary.Api.Hashing;
using ELibrary.src.ELibrary.Domain.RefreshTokenModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using ELibrary.src.ELibrary.Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.src.ELibrary.Infrastructure.Data.UserModel
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ELibraryDbContext _dbContext;
        //private readonly IUnitOfWork _unitOfWork;

        public AuthRepository(ELibraryDbContext dbContext)
        {
            _dbContext = dbContext;
            //_unitOfWork = unitOfWork;
        }

        public async Task<User> Logination(string email, string password)
        {
            var user = await _dbContext.User.SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                throw new Exception("User with this email did not exist");
            }
            if (user.Password != Hashing.Hash(password))
            {
                throw new Exception("Incorrect password");
            }
            return user;
           // throw new NotImplementedException();
        }

        public async Task LogOut(int userId)
        {
            var user = await _dbContext.User.SingleOrDefaultAsync(x => x.Id  == userId);
            var token = await _dbContext.Tokens
                .SingleOrDefaultAsync(x => x.UserId == userId);
            var newToken = new RefreshToken(userId, String.Empty, token.IssuedAt,
                token.ExpiresAt);
            _dbContext.Tokens.Remove(token);
            await _dbContext.Tokens.AddAsync(newToken);
            //_dbContext.Tokens.Update(token);
            //_unitOfWork.Commit();
            //throw new NotImplementedException();
        }

        public async Task Registration(RefreshToken token)
        {
            await _dbContext.Tokens.AddAsync(token);
            //var entity = await _dbContext.User.AddAsync(user);
            //return entity.Entity;
            //_unitOfWork.Commit();
            //throw new NotImplementedException();
        }

        public async Task UpdateRefreshToken(RefreshToken newToken)
        {
            var token = await _dbContext.Tokens
                .SingleOrDefaultAsync(x => x.UserId == newToken.UserId);
            _dbContext.Tokens.Remove(token);
            await _dbContext.Tokens.AddAsync(newToken);
            //token.Edit(newToken.Token, newToken.IssuedAt, newToken.ExpiresAt);
            //    _dbContext.Tokens.Update(token);
            //throw new NotImplementedException();
        }
    }
}
