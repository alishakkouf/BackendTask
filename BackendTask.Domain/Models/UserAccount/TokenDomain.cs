using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTask.Domain.Models.UserAccount
{
    public class TokenDomain
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        /// <summary>
        /// Token expiration in seconds
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// The bearer access token (expiration equals ExpiresIn)
        /// </summary>
        public string AccessToken { get; set; }

        public List<RoleDomain> Roles { get; set; }
    }
}
