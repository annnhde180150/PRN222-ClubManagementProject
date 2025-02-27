using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models
{
    public class Club
    {
        [Key]
        public int ClubId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ClubName { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<ClubMember> ClubMembers { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
