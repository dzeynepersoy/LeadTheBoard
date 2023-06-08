using System.ComponentModel.DataAnnotations;

namespace LeadTheBoard.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = null;

    }
}
