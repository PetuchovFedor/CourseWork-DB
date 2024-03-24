namespace ELibrary.src.ELibrary.Api.Dto
{
    public record UserLksDto
    (
        int Id,
        string Name,
        string? About,
        string PhotoPath,
        string Email,
	    DateTime DateRegistration
        //List<BookIconDto> UserBooks  
    );
}
