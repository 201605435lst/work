using SnAbp.Problem.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Problem.IServices
{
    public interface IProblemProblemCategoryAppService : IApplicationService
    {
        Task<ProblemCategoryDto> Get(Guid id);
        Task<PagedResultDto<ProblemCategoryDto>> GetList(ProblemCategorySearchDto input);
        Task<ProblemCategoryDto> Create(ProblemCategoryCreateDto input);
        Task<ProblemCategoryDto> Update(ProblemCategoryUpdateDto input);
        Task<bool> Delete(Guid id);
    }
}
