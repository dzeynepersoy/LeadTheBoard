using LeadTheBoard.Domain.Entities.Base;

namespace LeadTheBoard.Domain.Entities
{
    public class Machine : BaseEntity
    {
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        //İlişkiler
        public ICollection<Operation> Operations { get; set; }

    }
}
