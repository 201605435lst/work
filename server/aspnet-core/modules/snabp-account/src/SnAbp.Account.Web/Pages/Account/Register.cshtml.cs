using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Account.Settings;
using Volo.Abp.Auditing;
using Volo.Abp.Application.Dtos;
using SnAbp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using IdentityUser = SnAbp.Identity.IdentityUser;
using Volo.Abp;

namespace SnAbp.Account.Web.Pages.Account
{
    public class RegisterModel : AccountPageModel
    {
        protected IAppAccountAppService AccountAppService { get; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty]
        public PostInput Input { get; set; }

        public RegisterModel(IAppAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            await CheckSelfRegistrationAsync();

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            await CheckSelfRegistrationAsync();

            var registerDto = new RegisterDto
            {
                AppName = "MVC",
                EmailAddress = Input.EmailAddress,
                Password = Input.Password,
                UserName = Input.UserName
            };

            var userDto = await AccountAppService.RegisterAsync(registerDto);
            var user = await UserManager.GetByIdAsync(userDto.Id);

            await UserManager.SetEmailAsync(user, Input.EmailAddress);

            await SignInManager.SignInAsync(user, isPersistent: false);

            return Redirect(ReturnUrl ?? "~/"); //TODO: How to ensure safety? IdentityServer requires it however it should be checked somehow!
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled) ||
                !await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }

        public class PostInput
        {
            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
            public string EmailAddress { get; set; }

            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string Password { get; set; }
        }
    }
}
