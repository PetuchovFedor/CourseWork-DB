using ELibrary.src.ELibrary.Domain.CommentModel;
using ELibrary.src.ELibrary.Domain.RatingModel;
using ELibrary.src.ELibrary.Domain.RoleModel;
using ELibrary.src.ELibrary.Domain.UserReadBookModel;
using ELibrary.src.ELibrary.Domain.UserWriteBookModel;
using System.ComponentModel.DataAnnotations;

namespace ELibrary.src.ELibrary.Domain.UserModel
{
    public partial class User
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? AboutUser { get; set; }
        public string Password { get; set; } = null!;
        public DateTime DateRegistration { get; set; }
        public string PhotoUser { get; set; } = null!;

        public virtual Role IdRoleNavigation { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }

        //protected User()
        //{
        //}

        public User(string name, int roleId, string email, string password, string? aboutUser,
            DateTime dateRegistration, string photoUser)
        {
            Name = name;
            RoleId = roleId;
            Email = email;
            DateRegistration = dateRegistration;
            Password = password;
            AboutUser = aboutUser;
            PhotoUser = photoUser;
            Comments = new HashSet<Comment>();
        }
    }
}
