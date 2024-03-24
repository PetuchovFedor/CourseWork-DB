using ELibrary.src.ELibrary.Api.Dto;

namespace ELibrary.src.ELibrary.Domain.UserModel
{
    public record FindUserResult
    (
        List<User> Users,
        int TotalNumber
    );
}
