using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeadTheBoard.Shared.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Full Name")]
        public string FullName { get; set; } = "";

        [Required]
        [DisplayName("Email")]
        public string Email { get; set; } = "";

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; } = "";

        [DisplayName("Image")]
        public string? ImageUrl { get; set; } = "";

        public DateTime LastActivity { get; set; }

        /// <summary>
        /// Kullanıcının rolü
        /// </summary>
        public string Title { get; set; } = "";

        [Required]
        [DisplayName("Title")]
        public int TitleId { get; set; }
    }
}
