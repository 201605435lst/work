using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    /// <summary>
    /// 年月表日计划
    /// </summary>
    public class DailyPlanDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 年月表计划主键
        /// </summary>
        public Guid PlanId { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime PlanDate { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>
        public decimal Count { get; set; }

        /// <summary>
        /// 计划类型
        /// </summary>
        public int PlanType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
