using SnAbp.Material.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Material.IServices
{
    public interface IMaterialSupplierAppService : IApplicationService
    {
        Task<SupplierDto> Get(Guid id);
        Task<PagedResultDto<SupplierSimpleDto>> GetList(SupplierSearchDto input);
        Task<SupplierSimpleDto> Create(SupplierCreateDto input);
        Task<SupplierSimpleDto> Update(SupplierUpdateDto input);
        Task<bool> Delete(Guid id);
    }
}
