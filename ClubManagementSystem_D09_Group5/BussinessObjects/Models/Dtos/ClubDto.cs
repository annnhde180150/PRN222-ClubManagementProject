using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models.Dtos
{
    public class ClubDetailsViewDto
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; } = string.Empty;
        public byte[]? Logo { get; set; }
        public byte[]? Cover { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<ClubMemberDto> ClubMembers { get; set; } = new List<ClubMemberDto>();
        public List<PostDto> Posts { get; set; } = new List<PostDto>();
    }

}
