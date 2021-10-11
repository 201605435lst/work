using SnAbp.Resource.Dtos;
using SnAbp.Resource.Dtos.StoreHouse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Store.IServices
{
    public interface IResourceStoreHouseAppService:IApplicationService
    {
        Task<StoreHouseDto> Get(Guid id);
        Task<PagedResultDto<StoreHouseDto>> GetList(StoreHouseSearchDto input);
        Task<StoreHouseDto> Create(StoreHouseCreateDto input);
        Task<StoreHouseDto> Update(StoreHouseUpdateDto input);
        Task<StoreHouseDto> UpdateEnable(StoreHouseUpdateEnableDto input);
        Task<bool> Delete(Guid id);
   
    }
}
