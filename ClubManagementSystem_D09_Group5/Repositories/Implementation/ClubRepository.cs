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
    public class ClubRepository : IClubRepository
    {
        private readonly FptclubsContext _context;

        public ClubRepository(FptclubsContext context)
        {
            _context = context;
        }

        public async Task<Club> GetClubByIdWithMembersPostsAsync(int clubId)
        {
            return await _context.Clubs
                .Include(c => c.ClubMembers)
                    .ThenInclude(cm => cm.User)
                .Include(c => c.ClubMembers)
                    .ThenInclude(m => m.Posts)
                .FirstOrDefaultAsync(c => c.ClubId == clubId);
        }
    }
}
