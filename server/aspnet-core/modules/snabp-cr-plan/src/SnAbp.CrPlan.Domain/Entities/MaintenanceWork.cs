using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 维修作业
    /// </summary>
    public class MaintenanceWork : CreationAuditedEntity<Guid>, IRepairTag
    {
        /// <summary>
        /// 提交车间机构
        /// </summary>
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

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
        /// 维修等级
        /// </summary>
        //public RepairLevel RepairLevel { get; set; }
        public string RepairLevel { get; set; }

        /// <summary>
        /// 维修类型
        /// </summary>
        public PlanType MaintenanceType { get; set; }

        /// <summary>
        /// 第一阶段审批流程id
        /// </summary>
        public Guid? ARKey { get; set; }

        /// <summary>
        /// 第二阶段审批流程id
        /// </summary>
        public Guid? SecondARKey { get; set; }

        /// <summary>
        /// 关联关系
        /// </summary>
        public virtual List<MaintenanceWorkRltSkylightPlan> MaintenanceWorkRltSkylightPlans { get; set; }

        /// <summary>
        /// 关联文件
        /// </summary>
        public virtual List<MaintenanceWorkRltFile> MaintenanceWorkRltFiles { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionary RepairTag { get; set; }

        public MaintenanceWork() { }
        public MaintenanceWork(Guid id) => Id = id;
    }
}
