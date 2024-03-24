using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.UserModel;

namespace ELibrary.src.ELibrary.Domain.UserWriteBookModel
{
    public class UserWriteBook
    {
        public int UserId { get; set; }
        public int BookId { get; set; }

        public virtual Book IdBookNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
        protected UserWriteBook() { }

        public UserWriteBook(int userId, int bookId, Book book, User user)
        {
            UserId = userId;
            BookId = bookId;
            IdBookNavigation = book;
            IdUserNavigation = user;
        }
    }
}
