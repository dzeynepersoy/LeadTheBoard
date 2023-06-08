using LeadTheBoard.Domain.Entities.Base;

namespace LeadTheBoard.Domain.Entities
{
    /// <summary>
    /// Kullanıcının kazandığı rozetlerin tutulduğu tablo
    /// </summary>
    public class UserBadges : BaseEntity
    {
        public User User { get; set; }
        public int UserId { get; set; }

        public Badge Badge { get; set; }
        public int BadgeId { get; set; }
        
    }
}
