namespace LeadTheBoard.Shared.Models.Operation
{
    public class OperationModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        //İşlem puanı 100 üzerinden olacak
        public int Point { get; set; } = 0;

        //İşlem zorluğu 5 üzerinden olacak
        public int DifficultyLevel { get; set; } = 0;

        public int? ProductId { get; set; } = 0;

        public int? MachineId { get; set; } = 0;

    }
}
