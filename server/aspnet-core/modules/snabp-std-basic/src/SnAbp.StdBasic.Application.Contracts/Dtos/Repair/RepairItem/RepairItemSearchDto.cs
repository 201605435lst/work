using System;
using System.Collections.Generic;
using SnAbp.StdBasic.Enums;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 维护分组
    /// </summary>
    public class RepairItemSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 是否是月表
        /// </summary>
        public bool? IsMonth { get; set; }

        /// <summary>
        /// 构件分类Ids
        /// </summary>
        public List<Guid> ComponentCategoryIds { get; set; }

        /// <summary>
        /// 顶级分组（设备类型） Id
        /// </summary>
        public Guid? TopGroupId { get; set; }

        /// <summary>
        /// 分组（设备名称） Id
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// 维修类型
        /// </summary>
        public RepairType? Type { get; set; }

        /// <summary>
        /// 关键字：内容，备注
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 执行单位
        /// </summary>
        public Guid? ExecutiveUnitId { get; set; }
        public string RepairTagKey { get; set; }

    }
}
