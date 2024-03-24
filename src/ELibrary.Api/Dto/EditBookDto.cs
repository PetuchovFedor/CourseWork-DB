namespace ELibrary.src.ELibrary.Api.Dto
{
    public record EditBookDto
    (
        int Id,
        string Name,
        string Annotation
        //IFormFile CoverPath,
        //IFormFile BookPath
    );
}
