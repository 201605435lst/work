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
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace SnAbp.Identity.Web.Pages.Identity.Users
{
    public class EditModalModel : IdentityPageModel
    {
        [BindProperty]
        public UserInfoViewModel UserInfo { get; set; }

        [BindProperty]
        public AssignedRoleViewModel[] Roles { get; set; }

        protected IIdentityUserAppService IdentityUserAppService { get; }

        public EditModalModel(IIdentityUserAppService identityUserAppService)
        {
            IdentityUserAppService = identityUserAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync(Guid id)
        {
            UserInfo = ObjectMapper.Map<IdentityUserDto, UserInfoViewModel>(await IdentityUserAppService.GetAsync(id));

            Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>((await IdentityUserAppService.GetAssignableRolesAsync()).Items);

            var userRoleIds = (await IdentityUserAppService.GetRolesAsync(UserInfo.Id, null)).Items.Select(r => r.Id).ToList();
            foreach (var role in Roles)
            {
                if (userRoleIds.Contains(role.Id))
                {
                    role.IsAssigned = true;
                }
            }
            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<UserInfoViewModel, IdentityUserUpdateDto>(UserInfo);
            input.RoleIds = Roles.Where(r => r.IsAssigned).Select(r => r.Id).ToArray();
            await IdentityUserAppService.UpdateAsync(UserInfo.Id, input);

            return NoContent();
        }

        public class UserInfoViewModel : IHasConcurrencyStamp
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [HiddenInput]
            public string ConcurrencyStamp { get; set; }

            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
            public string UserName { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
            public string Name { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
            public string Surname { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string Password { get; set; }

            [Required]
            [EmailAddress]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
            public string Email { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
            public string PhoneNumber { get; set; }

            public bool TwoFactorEnabled { get; set; }

            public bool LockoutEnabled { get; set; }
        }

        public class AssignedRoleViewModel
        {
            public Guid Id { get; set; }
            [Required]
            [HiddenInput]
            public string Name { get; set; }

            public bool IsAssigned { get; set; }
        }
    }
}
