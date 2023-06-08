namespace LeadTheBoard.Shared.Models.Operation
{
    public class OperationListModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        //İşlem puanı 100 üzerinden olacak
        public int Point { get; set; } = 0;

        //İşlem zorluğu 5 üzerinden olacak
        public int DifficultyLevel { get; set; } = 0;

        public string ProductName { get; set; }

        public string MachineName { get; set; }
        public int MachineId { get; set; }
        public int ProductId { get; set; }
    }
}
