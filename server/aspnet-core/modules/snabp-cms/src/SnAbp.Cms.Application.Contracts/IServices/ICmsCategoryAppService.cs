using SnAbp.Cms.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Cms.IServices
{
    public interface ICmsCategoryAppService : IApplicationService
    {
        Task<CategoryDto> Get(Guid id);
        Task<CategoryDto> GetByCode(string code);
        Task<PagedResultDto<CategoryDto>> GetList(CategorySearchDto input);
        Task<CategorySimpleDto> Create(CategoryCreateDto input);
        Task<CategorySimpleDto> Update(CategoryUpdateDto input);
        Task<CategorySimpleDto> UpdateEnable(CategoryUpdateEnableDto input);
        Task<bool> Delete(Guid id);
    }
}
