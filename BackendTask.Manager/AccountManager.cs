using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackendTask.Domain.Contract.UserContract;
using BackendTask.Domain.Models.UserAccount;
using BackendTask.Shared.Exceptions;
using Microsoft.Extensions.Configuration;

namespace BackendTask.Manager
{
    public class AccountManager : IAccountManager
    {
        private readonly IAccountProvider _accountProvider;
        private IConfiguration Configuration { get; set; }

        public AccountManager(IAccountProvider accountProvider, IConfiguration Configuration)
        {
            _accountProvider = accountProvider;
            this.Configuration = Configuration;
        }

        public async Task<TokenDomain> LoginAsync(LoginInputDomain domain)
        {
            var user = await _accountProvider.LoginAsync(domain);

            var tokenDomain = JWTGenerator.GenerateJWTToken(new CreateTokenRequest
            {
                Email = user.UserName,
                IsActive = true,
                UserId = user.Id
            }, Configuration);

            tokenDomain.FullName = user.FullName;
            tokenDomain.UserId = user.Id;
            tokenDomain.UserName = user.UserName;

            return tokenDomain;
        }

        public async Task RegisterAsync(RegisterInputDomain Domain)
        {
            var existedUser = await _accountProvider.FindUserAsync(Domain.UserName);

            if (existedUser != null) throw new BusinessException("Email is already Existed!!");

             await _accountProvider.RegisterAsync(Domain);
        }

        public async Task<List<UserAccountDomain>> GetAllAsync()
        {
            return await _accountProvider.GetAllAsync();
        }

        public async Task UpdateUserAsync(UpdateUserDomain Domain)
        {
            var existedUser = await _accountProvider.FindUserAsync(Domain.UserName);

            if (existedUser != null && existedUser.Id != Domain.Id)
                throw new BusinessException("Existed Email!!");

            await _accountProvider.UpdateUserAsync(Domain);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _accountProvider.DeleteUserAsync(id);
        }
    }
}
