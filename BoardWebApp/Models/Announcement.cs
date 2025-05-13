using System.ComponentModel.DataAnnotations;

namespace BoardWebApp.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string SubCategory { get; set; }
    }
}
