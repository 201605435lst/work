using SnAbp.Basic.Entities;
using SnAbp.Basic.Enums;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 天窗计划
    /// </summary>
    public class SkylightPlan : Entity<Guid>, IRepairTag
    {
        public SkylightPlan() { }

        public SkylightPlan(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 级别RepairLevel枚举值，多选后逗号隔开
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 所属线路
        /// </summary>
        public Guid? RailwayId { get; set; }
        public virtual Railway Railway { get; set; }
        /// <summary>
        /// 所属车站
        /// </summary>
        public Guid Station { get; set; }
        /// <summary>
        /// 站点关联线路类型
        /// </summary>
        public RelateRailwayType StationRelateRailwayType { get; set; }
        /// <summary>
        /// 作业机房
        /// </summary>
        public List<SkylightPlanRltInstallationSite> WorkSites { get; set; }
        /// <summary>
        /// 位置（里程）
        /// </summary>
        [MaxLength(500)]
        public string WorkArea { get; set; }
        /// <summary>
        /// 计划时长
        /// </summary>
        public int TimeLength { get; set; }
        /// <summary>
        /// 计划日期
        /// </summary>
        public DateTime WorkTime { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>
        public string WorkContent { get; set; }
        /// <summary>
        /// 计划内容类型
        /// </summary>
        public WorkContentType? WorkContentType { get; set; }
        /// <summary>
        /// 影响范围
        /// </summary>
        //[MaxLength(500)]
        public string Incidence { get; set; }
        /// <summary>
        /// 作业单位(车间)
        /// </summary>
        public Guid WorkUnit { get; set; }
        /// <summary>
        /// 作业工区(其他计划)
        /// </summary>
        public Guid? WorkAreaId { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public Guid? SubmitUser { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public Guid? ResponsibleUser { get; set; }
        /// <summary>
        /// 制表人
        /// </summary>
        public Guid? CreateUser { get; set; }
        /// <summary>
        /// 制表时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; set; }
        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanType PlanType { get; set; }
        /// <summary>
        /// 计划状态
        /// </summary>
        public PlanState PlanState { get; set; }
        /// <summary>
        /// 退回意见
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 登记地点
        /// </summary>
        public string RegistrationPlace{ get; set; }

        /// <summary>
        /// 是否为相邻区间
        /// </summary>
        public bool IsAdjacent { get; set; }

        /// <summary>
        /// 终止站点关联线路类型
        /// </summary>
        public RelateRailwayType EndStationRelateRailwayType { get; set; }

        /// <summary>
        /// 终止站点Id
        /// </summary>
        public Guid? EndStationId { get; set; }

        //是否变更
        public bool IsChange { get; set; }

        //变更后新计划的工作时间
        public string ChangTime { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        /// <summary>
        /// 是否上道
        /// </summary>
        public bool IsOnRoad { get; set; }
    }
}
