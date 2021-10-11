using SnAbp.Common.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Common.IServices
{
    public interface ICommonAreaAppService : IApplicationService
    {
        Task<List<AreaDto>> GetListByIds(List<string> list);

        Task<PagedResultDto<AreaDto>> GetList(AreaSearchDto dto);
    }
}
