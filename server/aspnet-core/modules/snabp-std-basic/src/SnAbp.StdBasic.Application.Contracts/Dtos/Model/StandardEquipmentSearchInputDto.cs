using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 标准设备
    /// </summary>
    public class StandardEquipmentSearchInputDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public Guid? ProductCategoryId { get; set; }

        /// <summary>
        /// 是否获取所有
        /// </summary>
        public bool IsAll { get; set; }
    }
}
