namespace ELibrary.src.ELibrary.Api.Dto
{
    public record CreateBookDto
    (
    //    name: string,
    //authors: string[],
    //annotation: string,
    //genres: number[],
    //cover: File,
    //bookFile: File
        string Name,
        List<string> Authors,
        string Annotation,
        List<int> Genres,
        IFormFile Cover,
        IFormFile BookFile
        //DateTime WritingDate,
    );
}
