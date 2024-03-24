namespace ELibrary.src.ELibrary.Api.Dto
{
    public record UpdatePasswordDto
    (
        int Id,
        string OldPassword,
        string NewPassword
    );
}
