using SnAbp.Material.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class PurchasePlanSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 审核状态（采购计划的审核状态）
        /// </summary>
        public PurchaseState State { get; set; }
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
        /// <summary>
        /// 是否计划选择模式
        /// </summary>
        public bool IsSelect { get; set; }
        /// <summary>
        /// 是否计划选择模式
        /// </summary>
        public bool IsTreeSelect { get; set; }

        public List<Guid> Ids { get; set; } = new List<Guid>();
    }
}
