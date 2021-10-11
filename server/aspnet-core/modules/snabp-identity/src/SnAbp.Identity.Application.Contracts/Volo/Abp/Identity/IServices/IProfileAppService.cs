using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Identity
{
    public interface IProfileAppService : IApplicationService
    {
        Task<ProfileDto> GetAsync();

        Task<ProfileDto> UpdateAsync(UpdateProfileDto input);

        Task ChangePasswordAsync(ChangePasswordInput input);
    }
}