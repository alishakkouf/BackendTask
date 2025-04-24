using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Domain.Models.UserAccount;

namespace BackendTask.Domain.Contract.UserContract
{
    public interface IAccountManager
    {
        /// <summary>
        /// Get list of users.
        /// </summary>
        Task<List<UserAccountDomain>> GetAllAsync();

        /// <summary>
        /// Login user and return token.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<TokenDomain> LoginAsync(LoginInputDomain loginInput);

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        Task RegisterAsync(RegisterInputDomain Domain);

        /// <summary>
        /// Update user info.
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        Task UpdateUserAsync(UpdateUserDomain Domain);

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteUserAsync(int id);
    }
}
