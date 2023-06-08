using LeadTheBoard.Domain.Entities.Base;

namespace LeadTheBoard.Domain.Entities
{
    public class Operation : BaseEntity
    {
        public string Name { get; set; } = "";

        //İşlem puanı 100 üzerinden olacak
        public int Point { get; set; } = 0;

        //İşlem zorluğu 5 üzerinden olacak
        public int DifficultyLevel { get; set; } = 0;

        //İlişkiler
        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Machine Machine { get; set; }
        public int MachineId { get; set; }
    }
}
