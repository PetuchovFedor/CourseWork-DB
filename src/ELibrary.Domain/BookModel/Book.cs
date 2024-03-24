using ELibrary.src.ELibrary.Domain.BooksGenresModel;
using ELibrary.src.ELibrary.Domain.CommentModel;
using ELibrary.src.ELibrary.Domain.RatingModel;
using ELibrary.src.ELibrary.Domain.UserReadBookModel;
using ELibrary.src.ELibrary.Domain.UserWriteBookModel;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ELibrary.src.ELibrary.Domain.BookModel
{
    public partial class Book
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Annotation { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public string Cover { get; set; } = null!;
        public string DownloadUrl { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; }
        protected Book()
        {
        }

        public Book(string name, DateTime publicationDate, 
            string cover, string annotation, string downloadUrl)
        {
            Name = name;
            PublicationDate = publicationDate;
            Cover = cover;
            Annotation = annotation;
            DownloadUrl = downloadUrl;
            Comments = new HashSet<Comment>();
        }
    }
}
