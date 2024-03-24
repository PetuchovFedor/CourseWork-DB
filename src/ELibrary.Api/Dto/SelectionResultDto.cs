namespace ELibrary.src.ELibrary.Api.Dto
{
    public record SelectionResultDto
    (
        List<BookIconDto> Books,
        int TotalNumber
    );
}
