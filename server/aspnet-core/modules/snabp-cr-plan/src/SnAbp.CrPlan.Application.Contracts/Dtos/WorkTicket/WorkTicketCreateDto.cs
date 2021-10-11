using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    public class WorkTicketCreateDto
    {
        /// <summary>
        /// 关联的垂直天窗的id
        /// </summary>
        public Guid SkylightPlanId { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        public string WorkTitle { get; set; }

        /// <summary>
        /// 作业地点
        /// </summary>
        public string WorkPlace { get; set; }

        /// <summary>
        /// 作业内容
        /// </summary>
        public string WorkContent { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartTime { get; set; }

        /// <summary>
        /// 影响范围
        /// </summary>
        public string InfluenceRange { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlanFinishTime { get; set; }

        /// <summary>
        /// 安全技术措施及注意事项
        /// </summary>
        public string SecurityMeasuresAndAttentions { get; set; }

        /// <summary>
        /// 制表人
        /// </summary>
        public string PaperMaker { get; set; }

        /// <summary>
        /// 作业负责人
        /// </summary>
        public string PersonInCharge { get; set; }

        public List<WorkTicketRltCooperationUnitDto> WorkTicketRltCooperationUnits { get; set; }

        public string CooperateContent { get; set; }

        /// <summary>
        /// 是否设置安全防护员
        /// </summary>
        public bool? SafeGuard { get; set; }
    }
}
