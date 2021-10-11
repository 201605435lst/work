using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    public class SkylightPlanSimpleUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 计划日期
        /// </summary>
        public DateTime PlanDate { get; set; }

        /// <summary>
        /// 计划时长
        /// </summary>
        public int TimeLength { get; set; }

        /// <summary>
        /// 保留的工作内容的id
        /// </summary>
        public List<Guid> SavedPlanDetialIds { get; set; } = new List<Guid>();
    }
}
