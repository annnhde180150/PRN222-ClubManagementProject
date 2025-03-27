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

        public async Task<Club> GetClubByIdWithMembersAsync(int clubId)
        {
            return await _context.Clubs
                .Include(c => c.ClubMembers)
                    .ThenInclude(cm => cm.User)
                .FirstOrDefaultAsync(c => c.ClubId == clubId);
        }

        public async Task AddClubAsync(Club club)
        {
            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();
        }
    }
}
