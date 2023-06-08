namespace LeadTheBoard.Shared.Models.Task
{
    public class TaskListModel
    {
        public int TaskAssignmentId { get; set; }
        public string TaskName { get; set; }
        public int Point { get; set; }
        public int DifficultyLevel { get; set; }
        public bool IsCompleted { get; set; }
    }
}
