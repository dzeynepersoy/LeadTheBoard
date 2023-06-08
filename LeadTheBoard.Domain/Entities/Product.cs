using LeadTheBoard.Domain.Entities.Base;

namespace LeadTheBoard.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = "";

        //İlişkiler
        public ICollection<Operation> Operations { get; set; }
        public ICollection<DepartmentAndProduct> DepartmentsAndProducts { get; set; }

    }
}
