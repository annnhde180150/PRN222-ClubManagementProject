using BussinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IClubMemberService
    {
        public Task<ClubMember> AddClubMemberAsync(ClubMember clubMember);
        Task<ClubMember> GetClubMemberById(int id);
        Task<IEnumerable<ClubMember>> GetClubMemberByUserId(int id);
        Task<ClubMember> GetClubMemberAsync(int clubID, int userId);
    }
}
