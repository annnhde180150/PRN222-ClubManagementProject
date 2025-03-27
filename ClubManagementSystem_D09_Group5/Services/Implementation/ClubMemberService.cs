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
    }
}
