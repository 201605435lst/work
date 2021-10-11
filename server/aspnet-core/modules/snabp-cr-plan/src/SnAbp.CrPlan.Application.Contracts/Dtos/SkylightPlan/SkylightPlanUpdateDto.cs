using SnAbp.Basic.Enums;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.SkylightPlan
{
    public class SkylightPlanUpdateDto : EntityDto<Guid>, IRepairTagKeyDto
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
        /// <summary>
        /// 作业机房
        /// </summary>
        public List<Guid>? WorkSiteIds { get; set; }
        /// <summary>
        /// 位置（里程）
        /// </summary>
        [MaxLength(500)]
        public string WorkArea { get; set; }
        /// <summary>
        /// 作业时长
        /// </summary>
        public int TimeLength { get; set; }
        /// <summary>
        /// 计划日期
        /// </summary>
        public DateTime WorkTime { get; set; }
        /// <summary>
        /// 影响范围
        /// </summary>
        //[MaxLength(500)]
        public string Incidence { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 作业工区(其他计划)
        /// </summary>
        public Guid? WorkAreaId { get; set; }
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
        /// 计划内容类型
        /// </summary>
        public string WorkContent { get; set; }
        /// <summary>
        /// 计划内容类型
        /// </summary>
        public WorkContentType WorkContentType { get; set; }
        /// <summary>
        /// 计划状态
        /// </summary>
        public PlanState PlanState { get; set; }
        /// <summary>
        /// 计划内容
        /// </summary>
        public List<PlanDetailUpdateDto> PlanDetails { get; set; }

        public string RepairTagKey { get; set; }

        /// <summary>
        /// 登记地点
        /// </summary>
        public string RegistrationPlace { get; set; }

        /// <summary>
        /// 是否为计划变更
        /// </summary>
        public bool IsChange { get; set; }


        /// <summary>
        /// 是否为相邻区间
        /// </summary>
        public bool IsAdjacent { get; set; }

        /// <summary>
        /// 终止站点关联线路类型
        /// </summary>
        public RelateRailwayType EndStationRelateRailwayType { get; set; }

        public bool IsOnRoad { get; set; }

        /// <summary>
        /// 终止站点Id
        /// </summary>
        public Guid? EndStationId { get; set; }
        public SkylightPlanUpdateDto() { }

        public SkylightPlanUpdateDto(Guid id)
        {
            Id = id;
        }

        //维修作业内容
        public MaintenanceWorkRltSkylightPlanDto MaintenanceQueryParams { get; set; }

    }
}
