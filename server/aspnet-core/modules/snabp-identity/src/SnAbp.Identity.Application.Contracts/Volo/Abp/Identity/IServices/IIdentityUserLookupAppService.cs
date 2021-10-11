using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using SnAbp.Users;

namespace SnAbp.Identity
{
    public interface IIdentityUserLookupAppService : IApplicationService
    {
        Task<UserData> FindByIdAsync(Guid id);

        Task<UserData> FindByUserNameAsync(string userName);

        Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input);
        
        Task<long> GetCountAsync(UserLookupCountInputDto input);
    }
}