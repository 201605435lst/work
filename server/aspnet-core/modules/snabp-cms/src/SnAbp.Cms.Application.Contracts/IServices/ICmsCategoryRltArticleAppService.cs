using SnAbp.Cms.Dto.CategoryRltArticle;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Cms.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Cms.IServices
{
    public interface ICmsCategoryRltArticleAppService : IApplicationService
    {
        Task<CategoryRltArticleSimpleDto> Get(Guid id);
        Task<PagedResultDto<CategoryRltArticleSimpleDto>> GetList(CategoryRltArticleSearchDto input);
        Task<CategoryRltArticleSimpleDto> Create(CategoryRltArticleCreateDto input);
        Task<CategoryRltArticleSimpleDto> Update(CategoryRltArticleUpdateDto input);
        Task<CategoryRltArticleSimpleDto> UpdateEnable(CategoryRltArticleUpdateEnableDto input);
        Task<CategoryRltArticleSimpleDto> UpdateOrder(CategoryRltArticleUpdateOrderDto input);
        Task<bool> Delete(List<Guid> ids);
    }
}
