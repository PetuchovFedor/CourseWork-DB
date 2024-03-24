using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.UserModel;

namespace ELibrary.src.ELibrary.Domain.UserReadBookModel
{
    public partial class UserReadBook
    {
        public int UserId { get; set; }
        public int BookId { get; set; }

        public virtual Book IdBookNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
        protected UserReadBook() { }

        public UserReadBook(int userId, int bookId)
        {
            UserId = userId;
            BookId = bookId;
        }
    }
}
