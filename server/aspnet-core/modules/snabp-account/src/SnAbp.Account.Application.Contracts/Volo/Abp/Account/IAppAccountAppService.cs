using System.Diagnostics;
using SnAbp.Identity;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using Volo.Abp.Application.Services;

namespace SnAbp.Account
{
    public interface IAppAccountAppService : IApplicationService
    {
        Task<IdentityUserDto> RegisterAsync(RegisterDto input);

        Task<bool> ResetAsync(ResetDto input);
    }
}