using SnAbp.Bpm;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.File;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    public class MaintenanceWorkCreateDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 提交车间机构
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 维修项目
        /// </summary>
        public string MaintenanceProject { get; set; }

        /// <summary>
        /// 维修类型
        /// </summary>
        public PlanType MaintenanceType { get; set; }

        /// <summary>
        /// 作业单位及负责人
        /// </summary>
        public string WorkOrgAndDutyPerson { get; set; }

        /// <summary>
        /// 签收单位
        /// </summary>
        public string SignOrganization { get; set; }

        /// <summary>
        /// 初审部门
        /// </summary>
        public string FirstTrial { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 维修等级
        /// </summary>
        //public RepairLevel RepaireLevel { get; set; }

        /// <summary>
        /// 计划状态
        /// </summary>
        public PlanState State { get; set; }

        /// <summary>
        /// 维修项标签
        /// </summary>
        public string RepairTagKey { get; set; }

        /// <summary>
        /// 维修项关联方案文件
        /// </summary>
        //public List<MaintenanceWorkRltFileCreateDto> MaintenanceWorkRltPlanFiles { get; set; } = new List<MaintenanceWorkRltFileCreateDto>();
        public List<FileDomainDto> MaintenanceWorkRltPlanFiles { get; set; } = new List<FileDomainDto>();

        /// <summary>
        /// 天窗计划Id
        /// </summary>
        public List<Guid> SkylightPlanIds { get; set; }
    }
}
