using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    public class YearMonthSearchDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 维修项标签
        /// </summary>
        public string RepairTag { get; set; }

        /// <summary>
        /// 生成年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// 生成车间
        /// </summary>
        public Guid? OrgId { get; set; }

        /// <summary>
        /// 生成类型(年表,月表,年度月表)
        /// </summary>
        public int PlanType { get; set; }

        /// <summary>
        /// 维修类型(集中检修/日常检修)
        /// </summary>
        public int? RepairlType { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
        public string RepairTagKey { get ; set ; }

        public bool IsAll { get; set; }
    }

    /// <summary>
    /// 获得可以变更的年月计划表
    /// </summary>
    public class YearMonthGetChangeDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 维修项标签
        /// </summary>
        public string RepairTag { get; set; }

        /// <summary>
        /// 变更组织机构
        /// </summary>
        public Guid OrgId { get; set; } 

        /// <summary>
        /// 生成类型(年表,月表,年度月表)
        /// </summary>
        public int PlanType { get; set; }

        /// <summary>
        /// 维修类型(集中检修/日常检修)
        /// </summary>
        public int? RepairlType { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
        public string RepairTagKey { get ; set ; }

        /// <summary>
        /// 新增需求：添加计划变更记录表
        /// </summary>
        public Guid? AlterRecordId { get; set; }

        public bool IsCreateRecord { get; set; }
    }   
}
