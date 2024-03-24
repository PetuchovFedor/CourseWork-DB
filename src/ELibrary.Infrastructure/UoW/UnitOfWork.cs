namespace ELibrary.src.ELibrary.Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ELibraryDbContext _eLibDbContext;

        public UnitOfWork(ELibraryDbContext eLibraryDbContext)
        {
            _eLibDbContext = eLibraryDbContext;
        }

        public void Commit()
        {
            _eLibDbContext.SaveChanges();
        }
    }
}
