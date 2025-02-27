using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [ForeignKey("ClubMember")]
        public int CreatedBy { get; set; }

        [Required]
        public string Content { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public ClubMember ClubMember { get; set; }
        public ICollection<PostInteraction> PostInteractions { get; set; }
    }
}
