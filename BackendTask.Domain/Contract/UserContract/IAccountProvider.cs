using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Domain.Models.UserAccount;

namespace BackendTask.Domain.Contract.UserContract
{
    public interface IAccountProvider
    {
        /// <summary>
        /// Get user by email with related entities. Throws <see cref="EntityNotFoundException"/> if not found.
        /// </summary>
        Task<UserAccountDomain> FindUserAsync(string email);

        /// <summary>
        /// Login user and return token.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<UserAccountDomain> LoginAsync(LoginInputDomain domain);

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task RegisterAsync(RegisterInputDomain command);

        /// <summary>
        /// Update user info.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task UpdateUserAsync(UpdateUserDomain command);

        /// <summary>
        /// Get list of users.
        /// </summary>
        Task<List<UserAccountDomain>> GetAllAsync();

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteUserAsync(int id);
    }
}
