using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [Required]
        public int PostID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(1000)]
        public string CommentText { get; set; } = null!;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("PostID")]
        public virtual Post Post { get; set; } = null!;

        [ForeignKey("UserID")]
        public virtual User User { get; set; } = null!;
    }
}
