using System.ComponentModel;

namespace LeadTheBoard.Shared.Models.Task
{
    public class TaskDetailModel
    {
        [DisplayName("Task ID")]
        public int TaskAssignmentId { get; set; }

        [DisplayName("Task Name")]
        public string TaskName { get; set; }

        [DisplayName("Task Point")]
        public int Point { get; set; }

        [DisplayName("Task Difficulty Level")]
        public int DifficultyLevel { get; set; }

        [DisplayName("Operator Name")]
        public string OperatorName { get; set; }

        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Machine Name")]
        public string MachineName { get; set; }

        [DisplayName("Operation Name")]
        public string OperationName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }


    }
}
