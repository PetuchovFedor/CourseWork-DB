namespace ELibrary.src.ELibrary.Api.Dto
{
    public record CreateCommentDto
    (
        int UserId,
        int BookId,
        string Content
    );
}
