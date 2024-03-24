using ELibrary.src.ELibrary.Domain.UserModel;
using System.ComponentModel.DataAnnotations;

namespace ELibrary.src.ELibrary.Domain.RefreshTokenModel
{
    public partial class RefreshToken
    {
        public RefreshToken(int userId, string token, DateTime issuedAt, DateTime expiresAt) 
        {
            UserId = userId;
            Token = token;
            IssuedAt = issuedAt;
            ExpiresAt = expiresAt;
        }
        public int UserId { get; set; }
        //[Key]
        public string Token { get; set; } = null!;
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public virtual User IdUserNavigation { get; set; } = null!;
        public void Edit(string token, DateTime issuedAt, DateTime expiresAt)
        {
            Token = token;
            IssuedAt = issuedAt;
            ExpiresAt = expiresAt;
        }
    }
}
