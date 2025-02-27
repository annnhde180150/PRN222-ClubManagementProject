using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models
{
    public class ClubMember
    {
        [Key]
        public int MembershipId { get; set; }

        [ForeignKey("Club")]
        public int ClubId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;

        public Club Club { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
