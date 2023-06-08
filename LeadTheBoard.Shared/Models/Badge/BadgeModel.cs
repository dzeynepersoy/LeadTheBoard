using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeadTheBoard.Shared.Models.Badge
{
    public class BadgeModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; } = "";
        
        [Required]
        [DisplayName("Description")]
        public string Description { get; set; } = "";
        
        public string ImageUrl { get; set; } = "";
        
        [Required]
        [DisplayName("Required Points")]
        public int RequiredPoints { get; set; } = 0;
        
        [Required]
        [DisplayName("Validity Date End")]
        public DateTime ValidityDateEnd { get; set; }
        
        [Required]
        [DisplayName("Validity Date Start")]
        public DateTime ValidityDateStart { get; set; }
    }
}
