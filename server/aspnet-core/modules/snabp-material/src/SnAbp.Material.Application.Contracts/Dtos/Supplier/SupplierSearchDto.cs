using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class SupplierSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 是否全部显示
        /// </summary>
        public bool IsAll { get; set; }
    }
}
