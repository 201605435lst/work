using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace SnAbp.Identity
{
    public abstract class IdentityUserCreateOrUpdateDtoBase : ExtensibleObject
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
        public string Surname { get; set; }


        string email;
        // [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string Email { get => string.IsNullOrEmpty(email) ? $"{UserName}@snabp.com" : email; set => email = value; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool LockoutEnabled { get; set; }

        public Guid? PositionId { get; set; }


        // public string[] RoleNames { get; set; }
        [CanBeNull]
        public Guid[] RoleIds { get; set; }
    }
}