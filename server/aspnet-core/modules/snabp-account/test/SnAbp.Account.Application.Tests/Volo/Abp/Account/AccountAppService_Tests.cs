using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using SnAbp.Identity;
using Xunit;

namespace SnAbp.Account
{
    public class AccountAppService_Tests : AbpAccountApplicationTestBase
    {
        private readonly IAppAccountAppService _accountAppService;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IdentityUserManager _userManager;
        
        public AccountAppService_Tests()
        {
            _accountAppService = GetRequiredService<IAppAccountAppService>();
            _identityUserRepository = GetRequiredService<IIdentityUserRepository>();
            _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
            _userManager = GetRequiredService<IdentityUserManager>();
        }

        [Fact]
        public async Task RegisterAsync()
        {
            var registerDto = new RegisterDto
            {
                UserName = "bob.lee",
                EmailAddress = "bob.lee@abp.io",
                Password = "P@ssW0rd",
                AppName = "MVC"
            };

            await _accountAppService.RegisterAsync(registerDto);

            var user = await _identityUserRepository.FindByNormalizedUserNameAsync(
                _lookupNormalizer.NormalizeName("bob.lee"));

            user.ShouldNotBeNull();
            user.UserName.ShouldBe("bob.lee");
            user.Email.ShouldBe("bob.lee@abp.io");

            (await _userManager.CheckPasswordAsync(user, "P@ssW0rd")).ShouldBeTrue();
        }
    }
}
