using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Identity;
using SnAbp.Identity.Web.Pages.Identity;
using Volo.Abp.Auditing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace SnAbp.Identity.Web.Pages.Identity.Users
{
    public class CreateModalModel : IdentityPageModel
    {
        [BindProperty]
        public UserInfoViewModel UserInfo { get; set; }

        [BindProperty]
        public AssignedRoleViewModel[] Roles { get; set; }

        protected IIdentityUserAppService IdentityUserAppService { get; }

        public CreateModalModel(IIdentityUserAppService identityUserAppService)
        {
            IdentityUserAppService = identityUserAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            UserInfo = new UserInfoViewModel();

            var roleDtoList = (await IdentityUserAppService.GetAssignableRolesAsync()).Items;

            Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>(roleDtoList);

            foreach (var role in Roles)
            {
                role.IsAssigned = role.IsDefault;
            }

            return Page();
        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<UserInfoViewModel, IdentityUserCreateDto>(UserInfo);
            input.RoleIds = Roles.Where(r => r.IsAssigned).Select(r => r.Id).ToArray();

            await IdentityUserAppService.CreateAsync(input);

            return NoContent();
        }

        public class UserInfoViewModel
        {
            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
            public string UserName { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
            public string Name { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
            public string Surname { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [DisableAuditing]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
            public string Password { get; set; }

            [Required]
            [EmailAddress]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
            public string Email { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
            public string PhoneNumber { get; set; }

            public bool TwoFactorEnabled { get; set; } = true;

            public bool LockoutEnabled { get; set; } = true;
        }

        public class AssignedRoleViewModel
        {
            public Guid Id { get; set; }

            [Required]
            [HiddenInput]
            public string Name { get; set; }

            public bool IsAssigned { get; set; }

            public bool IsDefault { get; set; }
        }
    }
}
