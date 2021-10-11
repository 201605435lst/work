using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace SnAbp.Identity
{
    public class SnAbpUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>, ITransientDependency
    {
        public SnAbpUserClaimsPrincipalFactory(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            IOptions<IdentityOptions> options) 
            : base(
                  userManager, 
                  roleManager, 
                  options)
        {
        }

        [UnitOfWork]
        public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await base.CreateAsync(user);

            if (user.TenantId.HasValue)
            {
                principal.Identities
                    .First()
                    .AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
            }

            return principal;
        }
    }
}