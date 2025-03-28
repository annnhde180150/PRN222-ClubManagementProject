﻿using BussinessObjects.Models;
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

        public async Task<ClubMember> AddClubMemberAsync(ClubMember clubMember)
        {
            await _context.ClubMembers.AddAsync(clubMember);
            await _context.SaveChangesAsync();
            return clubMember;
        }

        public async Task DeleteClubMemberAsync(int id)
        {
            var member = await GetClubMemberAsync(id);
            _context.ClubMembers.Remove(member);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClubMember>> GetClubMembersAsync()
        {
            return await _context.ClubMembers.Include(m => m.TaskAssignments).Include(m => m.User).ToListAsync();
        }

        public async Task<ClubMember> GetClubMemberAsync(int id)
        {
            return await _context.ClubMembers.Include(m => m.TaskAssignments).Include(m => m.User).Include(m => m.Role).FirstOrDefaultAsync(m => m.MembershipId == id);
        }

        public async Task UpdateClubMemberAsync(ClubMember clubMember)
        {
            _context.Entry(clubMember).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    
        public async Task<bool> IsUserInClubAsync(int userId, int clubId)
        {
            return await _context.ClubMembers
                .AnyAsync(cm => cm.UserId == userId && cm.ClubId == clubId);
        }
    }
}
