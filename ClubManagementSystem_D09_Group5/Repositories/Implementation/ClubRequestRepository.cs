﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.Implementation
{
    public class ClubRequestRepository : IClubRequestRepository
    {
        private readonly FptclubsContext _context;
        public ClubRequestRepository(FptclubsContext context)
        {
            _context = context;
        }
        public async Task<ClubRequest?> AddClubRequestAsync(ClubRequest clubRequest)
        {
            _context.ClubRequests.Add(clubRequest);
            await _context.SaveChangesAsync();
            return clubRequest;
        }

        public async Task <IEnumerable<ClubRequest?>> GetAllClubRequestPendingAsync()
        {
            return await _context.ClubRequests
                .Include(c => c.User)
                .Where(c => c.Status == "Pending")
                .ToListAsync();
        }
    }
}
