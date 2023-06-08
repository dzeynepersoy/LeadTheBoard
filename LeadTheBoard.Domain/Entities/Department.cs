using LeadTheBoard.Domain.Entities.Base;

namespace LeadTheBoard.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public ICollection<DepartmentAndProduct> DepartmentsAndProducts { get; set; }
    }
}
