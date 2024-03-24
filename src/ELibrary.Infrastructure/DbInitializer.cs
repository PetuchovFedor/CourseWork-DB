namespace ELibrary.src.ELibrary.Infrastructure
{
    public class DbInitializer
    {
        public static void Initialize(ELibraryDbContext eLibDbContext)
        {
            eLibDbContext.Database.EnsureCreated();
        }
    }
}
