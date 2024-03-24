namespace ELibrary.src.ELibrary.Api.Dto
{
    public record RatingDto
    (
        int UserId,
        int BookId,
        int Mark
    );
}
