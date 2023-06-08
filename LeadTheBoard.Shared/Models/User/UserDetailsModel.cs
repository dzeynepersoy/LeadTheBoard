using LeadTheBoard.Shared.Models.Badge;
using LeadTheBoard.Shared.Models.Task;

namespace LeadTheBoard.Shared.Models.User
{
    public class UserDetailsModel
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserImageUrl { get; set; }
        public string Title { get; set; }
        public List<BadgeListModel> Badges { get; set; }
        public List<TaskStatisticsModel> Statistics { get; set; }
    }
}
