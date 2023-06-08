namespace LeadTheBoard.Shared.Models.TaskAssignment
{
    public class TaskAssignmentListModel
    {
        public int Id { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; } = "";

        public int OperationId { get; set; }
        public string OperationName { get; set; } = "";

        public bool IsDone { get; set; } = false;

        public DateTime CreatedAt { get; set; }
    }
}
