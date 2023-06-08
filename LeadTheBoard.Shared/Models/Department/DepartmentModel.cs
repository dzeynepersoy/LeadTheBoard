namespace LeadTheBoard.Shared.Models.Department
{
    public class DepartmentModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public List<int> Products { get; set; } = new();
    }
}
