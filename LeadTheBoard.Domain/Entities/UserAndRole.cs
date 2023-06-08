using LeadTheBoard.Domain.Entities.Base;

namespace LeadTheBoard.Domain.Entities
{
    public class UserAndRole : BaseEntity
    {
        public User User { get; set; }
        public int UserId { get; set; }

        public Role Role { get; set; }
        public int RoleId { get; set; }
    }
}
