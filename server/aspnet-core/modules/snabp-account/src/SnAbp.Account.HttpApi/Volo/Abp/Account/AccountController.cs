using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using SnAbp.Identity;

namespace SnAbp.Account
{
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Area("account")]
    [ControllerName("AppAccount")]
    [Route("api/account")]
    public class AccountController : AbpController, IAppAccountAppService
    {
        protected IAppAccountAppService AccountAppService { get; }

        public AccountController(IAppAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }

        [HttpPost]
        [Route("register")]
        public virtual Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            return AccountAppService.RegisterAsync(input);
        }
        [HttpPost]
        [Route("reset")]
        public Task<bool> ResetAsync(ResetDto input)
        {
            return AccountAppService.ResetAsync(input);
        }
    }
}