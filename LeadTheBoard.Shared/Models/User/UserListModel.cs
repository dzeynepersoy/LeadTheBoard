namespace LeadTheBoard.Shared.Models.User
{
    public class UserListModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// Kullanıcının rolü
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Kullanıcının rolün id'si
        /// </summary>
        public int TitleId { get; set; }

        public DateTime LastActivity { get; set; }

    }
}
