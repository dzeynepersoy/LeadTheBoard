namespace LeadTheBoard.Shared.Models.LeadBoard
{
    public class LeadBoardModel
    {
        public LeadBoardUsersListModel CurrentUser { get; set; } = new();

        public LeadBoardUsersModel Users { get; set; } = new();
    }
}
