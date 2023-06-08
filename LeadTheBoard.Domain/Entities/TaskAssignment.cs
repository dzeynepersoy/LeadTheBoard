using LeadTheBoard.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeadTheBoard.Domain.Entities
{
    public class TaskAssignment : BaseEntity
    {
        public User Operator { get; set; }

        [ForeignKey("User")]
        public int OperatorId { get; set; }

        public Operation Operation { get; set; }

        [ForeignKey("Operation")]
        public int OperationId { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CompletedDateTime { get; set; }

    }
}
