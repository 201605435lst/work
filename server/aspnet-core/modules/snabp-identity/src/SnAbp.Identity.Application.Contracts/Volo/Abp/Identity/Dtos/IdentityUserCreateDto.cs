using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace SnAbp.Identity
{
    public class IdentityUserCreateDto : IdentityUserCreateOrUpdateDtoBase
    {
        [DisableAuditing]
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string Password { get; set; }

        public Guid? OrganizationId { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}