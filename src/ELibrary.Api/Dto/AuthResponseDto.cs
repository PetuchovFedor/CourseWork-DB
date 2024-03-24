namespace ELibrary.src.ELibrary.Api.Dto
{
    public record AuthResponseDto
    (
        string? AccessToken,
        string? RefreshToken
    );
}
