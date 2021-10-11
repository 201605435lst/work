using SnAbp.Cms.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Cms.IServices
{
    public interface ICmsArticleAppService : IApplicationService
    {
        Task<ArticleDto> Get(Guid id);
        Task<PagedResultDto<ArticleSimpleDto>> GetList(ArticleSearchDto input);
        Task<PagedResultDto<ArticleSimpleDto>> GetOutofCategoryList(ArticleSearchOutofCategoryDto input);
        Task<ArticleDto> Create(ArticleCreateDto input);
        Task<ArticleDto> Update(ArticleUpdateDto input);
        Task<bool> Delete(List<Guid> ids);
    }
}
