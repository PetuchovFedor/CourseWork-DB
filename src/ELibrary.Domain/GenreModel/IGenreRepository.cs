namespace ELibrary.src.ELibrary.Domain.GenreModel
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetGenreList();
        Task<Genre> GetById(int id);
        Task<Genre> Create(Genre genre);
        void Delete(Genre genre);
        void Update(Genre genre);
    }
}
