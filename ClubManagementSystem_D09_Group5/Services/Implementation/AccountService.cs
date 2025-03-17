﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObjects.Models;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implementation
{
    public class AccountService : IAccountService
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
        public Task<User?> CheckEmailExist(string email)
        {
            return _accountRepository.CheckEmailExist(email);
        }
        public Task<User> AddGmailUser (User user)
        {
            return _accountRepository.AddGmailUser(user);
        }
        public Task<User> AddUser(User user)
        {
            return _accountRepository.AddUser(user);
        }

        public Task<User?> CheckUsernameExist(string username)
        {
            return _accountRepository.CheckUsernameExist(username);
        }

        public Task<User?> FindUserAsync(int id)
        {
            return _accountRepository.FindUserAsync(id);
        }

        public Task<User?> UpdateUserAsync(User user)
        {
            return _accountRepository.UpdateUserAsync(user);
        }

        public string ConvertToBase64(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;
            }

            return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
        }
    }
}
