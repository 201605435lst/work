using SnAbp.Resource.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices
{
    public interface IResourceStoreEquipmentTestAppService : IApplicationService
    {
        Task<StoreEquipmentTestDto> Create(StoreEquipmentTestCreateDto input);
        Task<PagedResultDto<StoreEquipmentTestDto>> GetList(StoreEquipmentTestSearchDto input);

        Task<StoreEquipmentTestDto> Get(Guid id);
    }
}
