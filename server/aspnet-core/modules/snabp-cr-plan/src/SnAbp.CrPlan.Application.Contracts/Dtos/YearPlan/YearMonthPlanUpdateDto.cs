using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    /// <summary>
    /// 年月表计划
    /// </summary>
    public class YearMonthPlanUpdateDto : EntityDto<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 天窗类型(多个用逗号分隔)
        /// </summary>
        [MaxLength(100)]
        public string SkyligetType { get; set; }

        public string RepairTagKey { get ; set ; }
    }
}
