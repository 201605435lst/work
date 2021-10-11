using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.TenantManagement
{
    public interface ITenantAppService : ICrudAppService<TenantDto, Guid, GetTenantsInput, TenantCreateDto, TenantUpdateDto>
    {
        Task<string> GetDefaultConnectionStringAsync(Guid id);

        Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString);

        Task DeleteDefaultConnectionStringAsync(Guid id);
    }
}
