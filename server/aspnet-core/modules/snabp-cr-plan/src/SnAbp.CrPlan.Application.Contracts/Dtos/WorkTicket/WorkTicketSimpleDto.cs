using SnAbp.CrPlan.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    public class WorkTicketSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string WorkTitle { get; set; }

        /// <summary>
        /// 作业地点
        /// </summary>
        public string WorkPlace { get; set; }

        /// <summary>
        /// 施工维修等级
        /// </summary>
        public string RepairLevel { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlanFinishTime { get; set; }

        /// <summary>
        /// 主体车间名称
        /// </summary>
        public string MainWorkShopName { get; set; }

        /// <summary>
        /// 配合作业内容
        /// </summary>
        public string CooperateContent { get; set; }

        public Guid SkylightPlanId { get; set; }

        public Guid WorkTicketRltCooperationUnitId { get; set; }

        /// <summary>
        /// 配合车间实际结束时间
        /// </summary>
        public DateTime CooperateRealFinishTime { get; set; }

        /// <summary>
        /// 配合车间实际开始时间
        /// </summary>
        public DateTime CooperateRealStartTime { get; set; }

        public string Completion { get; set; }

        public WorkTicketRltCooperationUnitState State { get; set; }
    }
}
