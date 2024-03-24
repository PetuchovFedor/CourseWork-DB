namespace ELibrary.src.ELibrary.Api.Dto
{
    public record ChangeGenresBookDto
    (
        int BookId,
        List<int> GenresId
    );    
}
