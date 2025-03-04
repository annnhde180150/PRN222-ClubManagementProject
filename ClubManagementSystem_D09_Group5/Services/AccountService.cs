using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObjects.Models;
using Repositories;

namespace Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<User?> CheckLogin(string username, string password)
        {
            return _accountRepository.CheckLogin(username, password);
        }
        public Task<ClubMember?> CheckRole(int userId)
        {
            return _accountRepository.CheckRole(userId);
        }
    }
}
