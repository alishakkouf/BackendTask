using AutoMapper;
using BackendTask.API.Controllers.Tasks;
using BackendTask.Common;
using BackendTask.Domain.Contract;
using BackendTask.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using BackendTask.Domain.Models.UserAccount;
using BackendTask.Domain.Contract.UserContract;
using BackendTask.API.Controllers.Accounts.Dtos;
using Serilog;

namespace BackendTask.API.Controllers.Accounts
{
    public class AccountController(IMapper mapper, IAccountManager accountManager, IStringLocalizerFactory factory)
        : BaseApiController(factory)
    {
        private readonly IMapper _mapper = mapper;
        private readonly IAccountManager _accountManager = accountManager;

        /// <summary>
        /// Get access token via username and password.
        /// </summary>
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResultDomain>> LoginAsync([FromBody] LoginInputDomain input)
        {
            var result = await _accountManager.LoginAsync(input);

            var toBeReturned = new LoginResultDomain
            {
                Id = result.UserId,
                FullName = result.FullName,
                UserName = result.UserName,
                AccessToken = result.AccessToken,
                ExpiresIn = result.ExpiresIn,
                Roles = result.Roles
            };

            return Ok(toBeReturned);
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        [Authorize]
        [HttpPost("AddUser")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterInputDto input)
        {
            Log.Information($"Register user {input.UserName}");
            await _accountManager.RegisterAsync(_mapper.Map<RegisterInputDomain>(input));

            return Ok();
        }

        /// <summary>
        /// update user info.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("UpdateUser")]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto input)
        {
            await _accountManager.UpdateUserAsync(_mapper.Map<UpdateUserDomain>(input));

            return Ok();
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var users = await _accountManager.GetAllAsync();

            return Ok(_mapper.Map<List<UserAccountDomain>>(users));
        }

        /// <summary>
        /// Delete User.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("DeleteUser")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            await _accountManager.DeleteUserAsync(id);

            return Ok();
        }
    }
}

