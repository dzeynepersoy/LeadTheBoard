using LeadTheBoard.Domain.Entities.Base;

namespace LeadTheBoard.Domain.Entities
{
    public class Badge : BaseEntity
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageUrl { get; set; } = "";

        public DateTime ValidityDateStart { get; set; }
        public DateTime ValidityDateEnd { get; set; }
        public int RequiredPoints { get; set; } = 0;

        public ICollection<UserBadges> UserBadges { get; set; }

    }
}
