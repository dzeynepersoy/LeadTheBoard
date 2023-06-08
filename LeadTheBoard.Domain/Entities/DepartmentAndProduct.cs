using LeadTheBoard.Domain.Entities.Base;

namespace LeadTheBoard.Domain.Entities
{
    /// <summary>
    /// Departman ve ürün için çoka çok ilişkiyi sağlayan tablo
    /// </summary>
    public class DepartmentAndProduct : BaseEntity
    {
        public Department Department { get; set; }
        public int DepartmentId { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
