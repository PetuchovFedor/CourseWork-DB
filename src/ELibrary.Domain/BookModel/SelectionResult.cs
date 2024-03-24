namespace ELibrary.src.ELibrary.Domain.BookModel
{
    public record SelectionResult
    (
        List<Book> Books,
        int TotalNumber
    );
}
