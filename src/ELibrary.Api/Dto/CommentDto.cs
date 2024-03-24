namespace ELibrary.src.ELibrary.Api.Dto
{
    public record CommentDto
    (
        int Id,
        int UserId,
        int BookId,
        UserIconDto UserIcon,
        string Content
    );
}
