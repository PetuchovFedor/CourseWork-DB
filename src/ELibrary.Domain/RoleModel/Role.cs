using ELibrary.src.ELibrary.Domain.UserModel;

namespace ELibrary.src.ELibrary.Domain.RoleModel
{
    public partial class Role
    {
        public Role(string name)
        {
            Users = new HashSet<User>();
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
