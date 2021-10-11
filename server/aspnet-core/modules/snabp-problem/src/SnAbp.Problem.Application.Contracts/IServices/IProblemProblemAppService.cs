using SnAbp.Problem.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Problem.IServices
{
    public interface IProblemProblemAppService : IApplicationService
    {
        Task<ProblemDto> Get(Guid id);
        Task<PagedResultDto<ProblemDto>> GetList(ProblemSearchDto input);
        Task<ProblemDto> Create(ProblemCreateDto input);
        Task<ProblemDto> Update(ProblemUpdateDto input);
        Task<bool> Delete(Guid id);
    }
}
