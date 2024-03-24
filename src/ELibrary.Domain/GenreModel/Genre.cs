using ELibrary.src.ELibrary.Domain.BooksGenresModel;
using System.ComponentModel.DataAnnotations;

namespace ELibrary.src.ELibrary.Domain.GenreModel
{
    public partial class Genre
    {        
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        protected Genre()
        {
        }
        public Genre(string name)
        {
            Name = name;
        }
        public void Edit(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
