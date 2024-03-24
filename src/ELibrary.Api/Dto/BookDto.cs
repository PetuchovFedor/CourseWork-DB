namespace ELibrary.src.ELibrary.Api.Dto
{
    public record BookDto
    (     
        int Id,
        string Name,
        string Annotation,
        string Cover,
        //string DownloadUrl,
        DateTime PublicationDate,
        List<GenreDto> Genres,
        double Rating,
        List<UserMiniDto> Authors,
        List<CommentDto> Comments
    //string DownloadUrl
    );
}
