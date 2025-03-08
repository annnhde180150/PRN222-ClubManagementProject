using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FptclubsContext _context;
        public AccountRepository(FptclubsContext context)
        {
            _context = context;
        }
        public async Task<User?> CheckLogin(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                return user;
            }
            return null;
        }    
        public async Task<ClubMember?> CheckRole(int userId)
        {
            var clubMember = await _context.ClubMembers
                .Include(c => c.Role)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (clubMember != null)
            {
                return clubMember;
            }
            return null;
        }
        public async Task<User?> CheckEmailExist(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Email == email);
            if(user != null)
            {
                return user;
            }
            return null;
        }
       public async Task<User> AddGmailUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
