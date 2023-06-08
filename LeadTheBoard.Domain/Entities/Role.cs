using LeadTheBoard.Domain.Entities.Base;

namespace LeadTheBoard.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = "";

        public ICollection<UserAndRole> UsersAndRoles { get; set; }

    }
}
