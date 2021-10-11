using SnAbp.Material.Dtos;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Material.IServices
{
    public interface IMaterialEntryRecordAppService : IApplicationService
    {
        Task<EntryRecordDetailDto> Get(Guid id);
        Task<PagedResultDto<EntryRecordDto>> GetList(EntryRecordSearchDto input);
        Task<EntryRecordDto> Create(EntryRecordCreateDto input);
        Task<Stream> Export(Guid id);
    }
}
