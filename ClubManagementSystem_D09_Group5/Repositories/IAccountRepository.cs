using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObjects.Models;

namespace Repositories
{
    public interface IAccountRepository
    {
        Task<User?> CheckLogin(string username, string password);
        Task<ClubMember?> CheckRole(int userId);
        Task<User?> CheckEmailExist(string email);
        Task<User?> AddGmailUser(User user);
    }
}
