namespace LeadTheBoard.Shared.Models.LeadBoard
{
    public class LeadBoardUsersListModel
    {
        public int UserId { get; set; } = 0;
        public int Rank { get; set; } = 0;
        public string Name { get; set; } = "";
        public int Points { get; set; } = 0;
        public string UserImage { get; set; } = "";
    }
}