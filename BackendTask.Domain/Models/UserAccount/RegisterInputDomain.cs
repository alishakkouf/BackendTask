using System.ComponentModel.DataAnnotations;
using BackendTask.Shared;
using Microsoft.Extensions.Localization;

namespace BackendTask.Domain.Models.UserAccount
{
    public class RegisterInputDomain 
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
