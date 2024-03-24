namespace ELibrary.src.ELibrary.Api.Dto
{
    public record UserDto
    (
        int Id,
        string Name,
        string Email,
        string? About,
        string PhotoPath,
        DateTime DateRegistration
    );
}
