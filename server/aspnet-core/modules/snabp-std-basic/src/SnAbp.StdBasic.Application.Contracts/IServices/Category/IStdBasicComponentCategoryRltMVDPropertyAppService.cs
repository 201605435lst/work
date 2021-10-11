
using SnAbp.StdBasic.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices.ComponentCategory.ComponentCategoryMVD
{
    public interface IStdBasicComponentCategoryRltMVDPropertyAppService : IApplicationService
    {
        Task<PagedResultDto<ComponentCategoryRltMVDPropertyDto>> EditList(ComponentCategoryRltMVDPropertyCreateDto input);

        Task<PagedResultDto<ComponentCategoryRltMVDPropertyDto>> GetListByComponentCategoryId(ComponentCategoryRltMVDPropertySearchDto input);
    }
}
