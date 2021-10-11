using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices.InfluenceRange
{
    public interface IStdBasicInfluenceRangeAppService : IApplicationService
    {
        Task<PagedResultDto<InfluenceRangeDto>> GetList(InfluenceRangeSearchDto input);
        Task<InfluenceRangeDto> Get(Guid id);
        Task<InfluenceRangeDto> Create(InfluenceRangeCreateDto input);
        Task<InfluenceRangeDto> Update(InfluenceRangeUpdateDto input);
        Task<bool> Delete(Guid id);
    }
}
