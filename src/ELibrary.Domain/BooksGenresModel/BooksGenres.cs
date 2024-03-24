using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.GenreModel;

namespace ELibrary.src.ELibrary.Domain.BooksGenresModel
{
    public partial class BooksGenres
    {
        public int BookId { get; set; }
        public int GenreId { get; set; }

        public virtual Book IdBookNavigation { get; set; } = null!;
        public virtual Genre IdGenreNavigation { get; set; } = null!;
        public BooksGenres(int bookId, int genreId)
        {
            BookId = bookId;
            GenreId = genreId;
        }
    }
}
