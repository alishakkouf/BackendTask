using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTask.Shared
{
    public static class Constants
    {
        public const string JsonContentType = "application/json";

        //public const string EmailRegularExpression = @"^([\w\.\-]+)@([\w\-]+\.)+(\w){2,}$";
        public const string EmailRegularExpression = @"^([\w\.\-]+)@([\w\-]+\.?)+(\w){2,}$";
        public const string PhoneCountryCodeExpression = @"^(\+)[1-9]\d{0,3}$";
        public const string PhoneRegularExpression = @"^(0|00|\+)[1-9]\d{5,14}$";
        public const string UserNameRegularExpression = @"^([\w\.\-]+)@([\w\-]+\.)*[\w\-]{2,}$";
        public const string DomainNameRegularExpression = @"^([\w\-]+\.)*[\w\-]{2,}$";
        public const string HexColorRegularExpression = @"^#([a-fA-F0-9]{3}){1,2}$";
        public const string EnglishCharactersRegex = @"^[a-zA-Z]*$";
        public const string CharactersRegularExpression = @"^[a-zA-Z \u00E4\u00F6\u00FC\u00C4\u00D6\u00DC\u00df]*$";
        public const string ArabicOrEnglishOrGermanCharactersRegex = @"^[a-zA-Z \u00E4\u00F6\u00FC\u00C4\u00D6\u00DC\u00df\u0621-\u065f]*$";
        public const string ArabicOrEnglishCharactersRegex = @"^[\u0621-\u065F\u066E-\u06D3a-zA-Z]+$";

        public const string AdministratorUserName = "Administrator";
        public const string DefaultPassword = "P@ssw0rd";
        public const string CustomerPasswardSalt = "Aa@.zs@s^";
        public const string DefaultPhoneNumber = "0";

        public const string SuperAdminUserName = "SuperAdmin";
        public const string SuperAdminEmail = "superadmin@Abha.org";

        public const int DefaultPageIndex = 1;
        public const int DefaultPageSize = 10;
        public const int AllItemsPageSize = 1000;
        public const int DropdownPageSize = 100;

        /// <summary>
        /// Configuration Key for Server Address in application config file
        /// </summary>
        public const string AppServerRootAddressKey = "App:ServerRootAddress";

        /// <summary>
        /// The custom claim type for the role permissions
        /// </summary>
        public const string PermissionsClaimType = "Permissions";

        /// <summary>
        /// The custom claim type to insure active user
        /// </summary>
        public const string ActiveUserClaimType = "UserIsActive";

        public const string DevEnvironment = "Dev";
        public const string TestEnvironment = "Test";
        public const string StageEnvironment = "Stage";
        public const string CertEnvironment = "Cert";

    }
}
