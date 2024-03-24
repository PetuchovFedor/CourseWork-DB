namespace ELibrary.src.ELibrary.Api.Dto
{
    public record FindUserDto
    (
        List<UserIconDto> Users,
        int TotalNumber
    );
}
