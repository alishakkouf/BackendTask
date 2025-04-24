using BackendTask.Shared;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace BackendTask.API.Controllers.Accounts.Dtos
{
    public class UpdateUserDto : IValidatableObject
    {
        [Required]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var factory = validationContext.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(CommonResource));
            var results = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(FirstName))
                results.Add(ValidationError(nameof(FirstName), "Required"));

            if (string.IsNullOrWhiteSpace(LastName))
                results.Add(ValidationError(nameof(LastName), "Required"));

            if (!string.IsNullOrEmpty(FirstName) && FirstName.Length > 100)
                results.Add(ValidationError(nameof(FirstName), "ValidationErrorsMessage"));

            if (!string.IsNullOrEmpty(LastName) && LastName.Length > 100)
                results.Add(ValidationError(nameof(LastName), "ValidationErrorsMessage"));

            if (!string.IsNullOrEmpty(PhoneNumber) && PhoneNumber.Length > 20)
                results.Add(ValidationError(nameof(PhoneNumber), "ValidationErrorsMessage"));

            if (!string.IsNullOrEmpty(FirstName) && !ContainsOnlyValidNameCharacters(FirstName))
                results.Add(ValidationError(nameof(FirstName), "OnlyCharactersInput"));

            if (!string.IsNullOrEmpty(LastName) && !ContainsOnlyValidNameCharacters(LastName))
                results.Add(ValidationError(nameof(LastName), "OnlyCharactersInput"));

            if (!string.IsNullOrEmpty(PhoneNumber) && !IsValidPhoneNumber(PhoneNumber))
                results.Add(ValidationError(nameof(PhoneNumber), "OnlyPhoneRegularInput"));

            return results;

            ValidationResult ValidationError(string propertyName, string messageKey)
            {
                return new ValidationResult(messageKey, new[] { propertyName });
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return email.IndexOf('@') > 0 &&
                   email.IndexOf('@') < email.LastIndexOf('.') &&
                   email.LastIndexOf('.') < email.Length - 1;
        }

        private bool ContainsOnlyValidNameCharacters(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            foreach (var c in name)
            {
                if (char.IsLetter(c) || c == ' ' || c == '\'' || c == '-') continue;
                if (c >= 0x0600 && c <= 0x06FF) continue; // Arabic characters
                return false;
            }
            return true;
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
            return phoneNumber.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' ' || c == '(' || c == ')');
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            return password.Length >= 6 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }
    }
}
