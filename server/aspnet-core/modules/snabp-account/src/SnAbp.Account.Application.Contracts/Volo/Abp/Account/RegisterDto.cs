using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using SnAbp.Identity;
using Volo.Abp.Validation;

namespace SnAbp.Account
{
    public class RegisterDto
    {
        string userName;
        string emailAddress;
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName
        {
            get => userName;
            set => userName = value;
        }

        // [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string EmailAddress
        {
            get => string.IsNullOrEmpty(emailAddress) ? $"{userName}@SnAbp.com" : emailAddress;
            set => emailAddress = value;
        }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; }

        [Required]
        public string AppName { get; set; }
    }
}