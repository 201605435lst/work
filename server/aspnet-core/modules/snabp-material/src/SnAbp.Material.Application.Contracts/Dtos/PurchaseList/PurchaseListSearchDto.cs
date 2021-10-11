using SnAbp.Material.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class PurchaseListSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 采购类型
        /// </summary>
        public PurchaseListType? Type { get; set; }

        /// <summary>
        /// 采购清单状态
        /// </summary>
        public PurchaseState? State { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 是否查询审批数据（根据此字段过滤）
        /// </summary>
        public bool Approval { get; set; }
        /// <summary>
        /// 是否获取待我审批的数据
        /// </summary>
        public bool Waiting { get; set; }

    }
}
