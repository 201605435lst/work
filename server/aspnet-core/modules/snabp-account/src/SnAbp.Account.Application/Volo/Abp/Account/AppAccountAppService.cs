using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SnAbp.Account.Settings;
using Volo.Abp;
using Volo.Abp.Application.Services;
using SnAbp.Identity;
using Volo.Abp.Settings;

namespace SnAbp.Account
{
    public class AppAccountAppService : ApplicationService, IAppAccountAppService
    {
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IdentityUserManager UserManager { get; }

        public AppAccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository)
        {
            RoleRepository = roleRepository;
            UserManager = userManager;
        }

        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            await CheckSelfRegistrationAsync();

            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);

            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

            if (string.IsNullOrEmpty(input.EmailAddress))
            {
                input.EmailAddress = $"{input.UserName}@SeenSun.com";
            }
            await UserManager.SetEmailAsync(user, input.EmailAddress);
            await UserManager.AddDefaultRolesAsync(user);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Reset)]
        public async Task<bool> ResetAsync(ResetDto input)
        {
            // 只有再同一组织结构下才能重置指定用户的密码
            var user = await UserManager.GetByIdAsync(input.UserId);
            if (user == null) throw new UserFriendlyException("用户不存在");

            //// 获取当前用户，判断其类型
            //var currentUser =await UserManager.GetByIdAsync(CurrentUser.Id.GetValueOrDefault());

            //if (currentUser.Organizations.Any())
            //{
            //    // 不是系统用户
            //    var canReset = false;
            //    var currentUserOrganizations =currentUser.Organizations.Select(a=>a.Organization).ToList();
            //    var modifiedOrganizations =await UserManager.GetOrganizationsAsync(user.Id);
            //    // 当前的组织结构必须包含，有一个条件满足即可。
            //    currentUserOrganizations?.ForEach(a =>
            //    {
            //        if (!modifiedOrganizations.Any(b => b.Code.StartsWith(a.Code)))
            //        {
            //            return;
            //        }

            //        canReset = true;
            //        return;
            //    });

            //    if (canReset)
            //    {
            //        await UserManager.CheckPasswordAsync(user, input.Password);
            //        return true;
            //    }
            //    else
            //    {
            //        throw new UserFriendlyException("当前所属组织结构无法修复该用户密码!");
            //    }
            //}
            //else
            //{
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            // 是系统用户，直接可以修改所有的
            await UserManager.ResetPasswordAsync(user, token, input.Password);
            return true;
            //}

        }
    }
}