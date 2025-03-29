using BussinessObjects.Models;
using Repositories.Implementation;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ClubMemberService : IClubMemberService
    {
        private readonly IClubMemberRepository _clubMemberRepository;

        public ClubMemberService(IClubMemberRepository clubMemberRepository)
        {
            _clubMemberRepository = clubMemberRepository;
        }

        public async Task<ClubMember> AddClubMemberAsync(ClubMember clubMember)
        {
            return await _clubMemberRepository.AddClubMemberAsync(clubMember);
        }

        public async Task<ClubMember> GetClubMemberById(int id)
        {
            return await _clubMemberRepository.GetClubMemberAsync(id);
        }

        public async Task<IEnumerable<ClubMember>> GetClubMemberByUserId(int id)
        {
            var allClubMembers = await _clubMemberRepository.GetClubMembersAsync();
            var clubMembers = allClubMembers.Where(m => m.UserId == id).ToList(); ;
            return clubMembers;
        }

        public async Task<ClubMember> GetClubMemberAsync(int clubID, int userId)
        {
            return (await _clubMemberRepository.GetClubMembersAsync())
                .Where(m => m.ClubId == clubID && m.UserId == userId)
                .FirstOrDefault();
        }

        public async Task<bool> IsClubMember(int clubID, int userId)
        {
            return (await _clubMemberRepository.GetClubMembersAsync())
                .Where(m => m.ClubId == clubID && m.UserId == userId)
                .FirstOrDefault() != null;
        }

        public async Task<IEnumerable<ClubMember>> GetClubMembersAsync(int clubId)
        {
            return (await _clubMemberRepository.GetClubMembersAsync())
                .Where(m => m.ClubId == clubId);
        }
    }
}
