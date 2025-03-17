using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models.Dtos
{
    public class ClubMemberDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string ProfilePictureBase64 { get; set; } = string.Empty;

    }

}
