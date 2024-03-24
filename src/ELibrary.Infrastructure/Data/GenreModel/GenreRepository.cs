using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.GenreModel;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.src.ELibrary.Infrastructure.Data.GenreModel
{
    public class GenreRepository: IGenreRepository
    {
        private readonly ELibraryDbContext _dbContext;

        public GenreRepository(ELibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Genre> Create(Genre genre)
        {
            var entity = await _dbContext.Genre.AddAsync(genre);
            return entity.Entity;
        }

        public void Delete(Genre genre)
        {
            _dbContext.Genre.Remove(genre);
        }

        public async Task<Genre> GetById(int id)
        {
            return await _dbContext.Genre.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Genre>> GetGenreList()
        {
            return await _dbContext.Genre.ToListAsync();
        }

        public void Update(Genre genre)
        {
            _dbContext.Genre.Update(genre);
            //throw new NotImplementedException();
        }
    }
}
