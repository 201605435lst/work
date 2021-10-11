using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.AlterRecord
{
    public class DailyPlanAlterDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 变更表id
        /// </summary>
        public Guid AlterRecordId { get; set; }

        /// <summary>
        /// 日计划id
        /// </summary>
        public Guid DailyId { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 变更后数量
        /// </summary>
        public decimal AlterCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public Guid? RepairTagId { get; set; }

        public DataDictionaryDto RepairTag { get; set; }
    }
}
