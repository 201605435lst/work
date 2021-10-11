
using SnAbp.StdBasic.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices.ProductCategory.ProductCategoryMVD
{
    public interface IStdBasicProductCategoryRltMVDPropertyAppService : IApplicationService
    {
        Task<PagedResultDto<ProductCategoryRltMVDPropertyDto>> EditList(ProductCategoryRltMVDPropertyCreateDto input);

        Task<PagedResultDto<ProductCategoryRltMVDPropertyDto>> GetListByProductCategoryId(ProductCategoryRltMVDPropertySearchDto input);
    }
}
