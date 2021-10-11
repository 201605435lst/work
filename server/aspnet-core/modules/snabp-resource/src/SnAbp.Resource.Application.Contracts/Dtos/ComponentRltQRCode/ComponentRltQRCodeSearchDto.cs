using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class ComponentRltQRCodeSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备分类
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }

        /// <summary>
        /// 关键字 设备名称 设备编码
        /// </summary>
        public string Keyword { get; set; }


    }
}
