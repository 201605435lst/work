using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.AlterRecord
{
    public class SelectablePlansSearchDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 类型 年、半年、季度表
        /// </summary>
        public SelectablePlanType? Type { get; set; }

        /// <summary>
        /// 关键字 设备名称/工作内容
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 计划所属组织机构
        /// </summary>
        public Guid OrgId { get; set; }

        /// <summary>
        /// 计划所属年月
        /// </summary>
        //public DateTime Date { get; set; }

        /// <summary>
        /// 天窗类型
        /// </summary>
        public PlanType? SkylightType { get; set; }

        public int IsChange { get; set; }
        public string RepairTagKey { get ; set ; }
    }
}
