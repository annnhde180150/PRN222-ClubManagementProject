using BussinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAccountService
    {
        Task<User?> CheckLogin(string username, string password);
        Task<ClubMember?> CheckRole(int userId);
        Task<User?> CheckEmailExist(string email);
        Task<User> AddGmailUser(User user);
        Task<User> AddUser(User user);
        Task<User?> CheckUsernameExist(string username);
        Task<User?> FindUserAsync(int userId);

        Task<User?> UpdateUserAsync(User user);

        string ConvertToBase64(byte[] imageBytes);
    }

}
