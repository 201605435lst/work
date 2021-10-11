using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.Statistical
{
    /// <summary>
    /// 月表统计实体
    /// </summary>
    public class MonthStatisticalDto : IRepairTagDto
    {
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public decimal FinishCount { get; set; }

        /// <summary>
        /// 未完成数量
        /// </summary>
        public decimal UnFinishedCount { get; set; }

        /// <summary>
        /// 已变更数量
        /// </summary>
        public decimal ChangeCount { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool isFinish { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
