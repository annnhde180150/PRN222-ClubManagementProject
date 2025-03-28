using BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementation
{
    public class ClubMemberRepository : IClubMemberRepository
    {
        private readonly FptclubsContext _context;
        public ClubMemberRepository(FptclubsContext context)
        {
            _context = context;
        }

        public async Task<bool> IsUserInClubAsync(int userId, int clubId)
        {
            return await _context.ClubMembers
                .AnyAsync(cm => cm.UserId == userId && cm.ClubId == clubId);
        }
    }
}