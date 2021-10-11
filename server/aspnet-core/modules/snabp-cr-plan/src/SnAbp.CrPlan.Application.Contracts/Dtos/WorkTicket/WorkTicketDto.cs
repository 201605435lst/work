using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    public class WorkTicketDto : EntityDto<Guid>
    {
        /// <summary>
        /// 命令号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 填报时间
        /// </summary>
        public DateTime? FillInTime { get; set; }

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
        /// 作业内容
        /// </summary>
        public string WorkContent { get; set; }

        /// <summary>
        /// 影响范围
        /// </summary>
        public string InfluenceRange { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlanFinishTime { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStartTime { get; set; }

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? RealFinsihTime { get; set; }

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

        /// <summary>
        /// 技术科审核人
        /// </summary>
        public Guid? TechnicalCheckerId { get; set; }
        public virtual IdentityUserDto TechnicalChecker { get; set; }

        /// <summary>
        /// 安调科审核人
        /// </summary>
        public Guid? SafetyDispatchCheckerId { get; set; }
        public virtual IdentityUserDto SafetyDispatchChecker { get; set; }

        /// <summary>
        /// 工作完成情况
        /// </summary>
        public string FinishContent { get; set; }

        /// <summary>
        /// 工作情况 完成时间
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinish
        {
            get
            {
                if (!string.IsNullOrEmpty(FinishContent) && RealFinsihTime != null && RealStartTime != null) return true;
                return false;
            }
        }

        /// <summary>
        /// 关联天窗计划id
        /// </summary>
        public Guid SkylightPlanId { get; set; }

        public List<WorkTicketRltCooperationUnitDto> workTicketRltCooperationUnits { get; set; } = new List<WorkTicketRltCooperationUnitDto>();

        /// <summary>
        /// 主体车间名称
        /// </summary>
        public string MainWorkShopName { get; set; }

        /// <summary>
        /// 配合作业内容
        /// </summary>
        public string CooperateContent { get; set; }

        /// <summary>
        /// 配合车间完成情况
        /// </summary>
        public string Completion { get; set; }

        /// <summary>
        /// 是否设置安全防护员
        /// </summary>
        public bool? SafeGuard { get; set; }

        public bool? IsFine { get; set; }
    }
}
