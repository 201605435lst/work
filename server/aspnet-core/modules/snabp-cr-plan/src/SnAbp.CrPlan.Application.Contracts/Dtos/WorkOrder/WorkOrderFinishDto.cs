using SnAbp.CrPlan.Dto.Worker;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 派工单实体
    /// 完成派工单使用（无天窗内部数据操作）
    /// </summary>
    public class WorkOrderFinishDto : EntityDto<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 实际作业起始时间
        /// </summary>
        public DateTime StartRealityTime { get; set; }

        /// <summary>
        /// 实际作业终止时间
        /// </summary>
        public DateTime EndRealityTime { get; set; }

        /// <summary>
        /// 命令票号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 完成情况反馈
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// 相关设备列表
        /// </summary>
        public List<EquipmentPlanDetailUpdateDto> EquipmentList { get; set; }


        /// <summary>
        /// 编辑时返回的附带信息
        /// </summary>
        public List<ResInfo> FinishInfos { get; set; } = new List<ResInfo>();

        public string RepairTagKey { get ; set ; }

        /// <summary>
        /// 计划内容类型
        /// </summary>
        public WorkContentType? WorkContentType { get; set; }

        /// <summary>
        /// 兑现反馈
        /// </summary>
        public PlanState CashFeedBack { get; set; }

        public bool isAcceptance { get; set; }
    }
    public class ResInfo
    {
        public string OrderNumber { get; set; }
        public string Content { get; set; }
        public ResInfo(string num)
        {
            OrderNumber = num;
        }
    }
}
