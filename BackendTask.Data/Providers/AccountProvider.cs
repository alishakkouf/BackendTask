using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BackendTask.Data.Models.Identity;
using BackendTask.Domain.Contract.UserContract;
using BackendTask.Domain.Models.UserAccount;
using BackendTask.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;

namespace BackendTask.Data.Providers
{
    internal class AccountProvider : GenericProvider<UserAccount>, IAccountProvider
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly RoleManager<UserRole> _userRoleManager;

        public AccountProvider(BackendTaskDbContext dbContext,
            IMapper mapper,
            UserManager<UserAccount> userManager,
            RoleManager<UserRole> userRoleManager,
            IStringLocalizer<AccountProvider> localizer) : base(dbContext, mapper)
        {
            _userManager = userManager;
            _userRoleManager = userRoleManager;
        }

        public async Task<UserAccountDomain> LoginAsync(LoginInputDomain domain)
        {
            // Fetch the user from the database with associated roles
            var userEntity = await ActiveDbSet.AsNoTracking()
                                              .FirstOrDefaultAsync(u => u.UserName == domain.UserName)
                       ?? throw new EntityNotFoundException(nameof(UserAccount), domain.UserName);

            // Validate the password and check if rehashing is needed
            var verificationResult = ValidatePassword(domain.Password, userEntity.PasswordHash, out string newPasswordHash);

            if (verificationResult == PasswordVerificationResult.Failed)
                throw new BusinessException("Invalid username or password.");

            // Rehash and update the password if needed
            if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                userEntity.PasswordHash = newPasswordHash;
                await _dbContext.SaveChangesAsync();
            }

            // Return the user domain object
            return new UserAccountDomain
            {
                Id = userEntity.Id,
                FullName = $"{userEntity.FirstName} {userEntity.LastName}",
                UserName = userEntity.UserName
            };
        }

        private static PasswordVerificationResult ValidatePassword(string password, string storedHash, out string updatedHash)
        {
            var passwordHasher = new PasswordHasher<object>();

            var result = passwordHasher.VerifyHashedPassword(null, storedHash, password);

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                updatedHash = HashPassword(password);
                return result;
            }

            if (result == PasswordVerificationResult.Success)
            {
                updatedHash = storedHash; 
                return result;
            }

            // Check against legacy hashing algorithm
            if (HashPasswordV1(password) == storedHash)
            {
                updatedHash = HashPassword(password); 
                return PasswordVerificationResult.SuccessRehashNeeded;
            }

            updatedHash = null; 
            return PasswordVerificationResult.Failed;
        }

        private static string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>();
            return passwordHasher.HashPassword(null, password);
        }

        private static string HashPasswordV1(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public async Task RegisterAsync(RegisterInputDomain command)
        {
            var user = _mapper.Map<UserAccount>(command);
            user.IsActive = true;

            var identityResult = await _userManager.CreateAsync(user, command.Password);

            if (!identityResult.Succeeded)
                throw new BusinessException("Error Adding user info");

            await _userManager.AddToRoleAsync(user, "Guest");
        }

        public async Task<UserAccountDomain> FindUserAsync(string email)
        {
            var user = await ActiveDbSet.FirstOrDefaultAsync(x => x.Email == email);

            return _mapper.Map<UserAccountDomain>(user);
        }

        public async Task UpdateUserAsync(UpdateUserDomain command)
        {
            var user = await ActiveDbSet.Where(x => x.Id == command.Id).FirstOrDefaultAsync() ??
               throw new EntityNotFoundException(nameof(UserAccount), command.Id.ToString());

            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.Email = command.UserName;
            user.UserName = command.UserName;
            user.PhoneNumber = command.PhoneNumber;

            await _userManager.UpdateAsync(user);
        }

        public async Task<List<UserAccountDomain>> GetAllAsync()
        {
            var users = await ActiveDbSet.AsNoTracking().ToListAsync();

            return _mapper.Map<List<UserAccountDomain>>(users);
        }

        public async Task DeleteUserAsync(int id)
        {

            var user = await ActiveDbSet.Where(u => u.Id == id)
                                        .FirstOrDefaultAsync() ??
            throw new EntityNotFoundException(nameof(UserAccount), id.ToString());

            await RandomizeUserNameListAsync(user);

            await SoftDeleteAsync(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RandomizeUserNameListAsync(UserAccount user)
        {
            var random = new Random();

            user.UserName = "D" + user.Id + "_" + user.UserName;
            user.NormalizedUserName = "D" + user.Id + "_" + user.NormalizedUserName;

            user.Email = "D" + user.Id + "_" + user.Email;
            user.NormalizedEmail = "D" + user.Id + "_" + user.NormalizedEmail;

            user.PhoneNumber = user.Id + user.PhoneNumber;

        }
    }
}
