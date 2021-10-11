using SnAbp.Material.Dtos;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Material.IServices
{
    public interface IMaterialOutRecordAppService : IApplicationService
    {
        Task<OutRecordDto> Get(Guid id);
        Task<PagedResultDto<OutRecordDto>> GetList(OutRecordSearchDto input);
        Task<OutRecordDto> Create(OutRecordCreateDto input);
        Task<Stream> Export(Guid id);
    }
}
