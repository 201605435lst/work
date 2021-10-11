using SnAbp.Basic.Dtos;
using SnAbp.Basic.Entities;
using SnAbp.Basic.Enums;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.File;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.SkylightPlan
{
    public class SkylightPlanDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 级别RepairLevel枚举值，多选后逗号隔开
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 车站（区间）
        /// </summary>
        public Guid StationId { get; set; }
        public RelateRailwayType StationRelateRailwayType { get; set; }
        /// <summary>
        /// 所属线路
        /// </summary>
        public Guid? RailwayId { get; set; }
        public virtual Railway Railway { get; set; }
        /// <summary>
        /// 车站（区间）名称
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 作业机房
        /// </summary>
        public List<Guid> WorkSiteIds { get; set; }
        /// <summary>
        /// 作业机房名称
        /// </summary>
        public string WorkSiteName { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        public Guid WorkUnit { get; set; }
        public string WorkUnitName { get; set; }
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
        public WorkContentType WorkContentType { get; set; }
        /// <summary>
        /// 影响范围
        /// </summary>
        //[MaxLength(500)]
        public string Incidence { get; set; }
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
        /// 
        /// </summary>
        public string Opinion { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }

        /// <summary>
        /// 终止站点Id
        /// </summary>
        public Guid? EndStationId { get; set; }
        //是否变更
        public bool IsChange { get; set; }

        //变更后新计划的工作时间
        public string ChangTime { get; set; }

        public SkylightPlanDto() { }


        public SkylightPlanDto(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 天窗计划关联工作票
        /// </summary>
        public List<WorkTicketDto> SkylightPlanRltWorkTickets { get; set; } = new List<WorkTicketDto>();

        /// <summary>
        /// 关联维修计划的工作流Id
        /// </summary>
        public Guid? WorkFlowId { get; set; }

        //垂直天窗作计划方案
        public List<FileDomainDto> Files { get; set; }

        public string SchemeCoverName { get; set; }
    }
}
