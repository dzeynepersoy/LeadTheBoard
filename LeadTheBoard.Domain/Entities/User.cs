using LeadTheBoard.Domain.Entities.Base;

namespace LeadTheBoard.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Password { get; set; } = "";

        public string? ImageUrl { get; set; } = "";

        public DateTime LastActivity { get; set; }

        public ICollection<UserAndRole> UsersAndRoles { get; set; }
        public ICollection<UserBadges> UserBadges { get; set; }
        public ICollection<TaskAssignment> TaskAssignments { get; set; }
    }
}
