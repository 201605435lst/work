using SnAbp.Resource.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.IServices.StoreEquipmentTransfer
{
   public interface IResourceStoreEquipmentTransferAppService
    {
        Task<StoreEquipmentTransferDto> Get(Guid id);
        Task<PagedResultDto<StoreEquipmentTransferDto>> GetList(StoreEquipmentTransferSearchDto input);
        Task<StoreEquipmentTransferDto> Import(StoreEquipmentTransferSimpleDto input);
        //Task<bool> Delate(Guid id);

        Task<StoreEquipmentTransferDto> Export(StoreEquipmentTransferSimpleDto input);
        Task<List<StoreEquipmentSimpleDto>> GetEquipmentImport(StoreEquipmentSimpleSearchDto input);
        Task<List<StoreEquipmentSimpleDto>> GetEquipmentExport(StoreEquipmentExportDto input);
    }
}
