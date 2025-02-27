using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(255)]
        public string ProfilePicture { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<ClubMember> ClubMembers { get; set; }
        public ICollection<PostInteraction> PostInteractions { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<ClubRequest> ClubRequests { get; set; }
    }
}
