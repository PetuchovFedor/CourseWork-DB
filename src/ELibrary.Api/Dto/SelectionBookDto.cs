namespace ELibrary.src.ELibrary.Api.Dto
{
    public record SelectionBookDto
    (
        List<int> GenresIds, 
        int SortingType,
        int Scipped, 
        int AuthorId, 
        int ReaderId
    );
}
