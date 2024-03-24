using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.UserModel;

namespace ELibrary.src.ELibrary.Domain.RatingModel
{
    public partial class Rating
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int? Mark { get; set; }

        public virtual Book IdBookNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
        protected Rating() { }

        public Rating(int mark, int userId, int bookId)
        {
            UserId = userId;
            Mark = mark;
            BookId = bookId;
        }

        public void Edit(int mark, int userId, int bookId)
        {
            UserId = userId;
            Mark = mark;
            BookId = bookId;
        }
    }
}
