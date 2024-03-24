using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using System.ComponentModel.DataAnnotations;

namespace ELibrary.src.ELibrary.Domain.CommentModel
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime DateWriting { get; set; }

        public virtual Book IdBookNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;

        protected Comment()
        {

        }

        public Comment(string content, DateTime writingDate, int userId, int bookId)
        {
            Content = content;
            DateWriting = writingDate;
            UserId = userId;
            BookId = bookId;
        }
    }
}
