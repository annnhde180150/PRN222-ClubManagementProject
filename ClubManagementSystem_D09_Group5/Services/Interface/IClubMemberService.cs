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
        Task<ClubMember> GetClubMemberByIdAsync(int membershipId);
        Task<ClubMember> AddClubMemberAsync(ClubMember clubMember);
        Task<ClubMember?> GetClubMemberById(int id);
        Task<IEnumerable<ClubMember>> GetClubMemberByUserId(int id);
        Task<bool> IsUserInClubAsync(int userId, int clubId);
        Task<ClubMember> GetClubMemberAsync(int clubID, int userId);
        Task<bool> IsClubMember(int clubID, int userId);
        Task<IEnumerable<ClubMember>> GetClubMembersAsync(int clubId);
        Task<IEnumerable<ClubMember>> GetClubMembersByClubIdAsync(int id);
        Task<(bool success, string message)> UpdateClubMemberAsync(ClubMember clubMember);
    }
}
