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
    public class SkylightPlanDetailDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 级别RepairLevel枚举值，多选后逗号隔开
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 车站（区间）ID
        /// </summary>
        public Guid StationId { get; set; }

        public RelateRailwayType StationRelateRailwayType { get; set; }
        /// <summary>
        /// 车站（区间）名称
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 作业机房ID
        /// </summary>
        public List<Guid>? WorkSiteIds { get; set; }
        /// <summary>
        /// 作业机房名称
        /// </summary>
        public string WorkSiteName { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
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
        /// 所属线路
        /// </summary>
        public Guid? RailwayId { get; set; }
        public virtual Railway Railway { get; set; }

        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanType PlanType { get; set; }
        /// <summary>
        /// 计划内容类型
        /// </summary>
        public WorkContentType WorkContentType { get; set; }
        /// <summary>
        /// 计划内容类型
        /// </summary>
        public string WorkContent { get; set; }
        /// <summary>
        /// 计划状态
        /// </summary>
        public PlanState PlanState { get; set; }
        /// <summary>
        /// 计划内容
        /// </summary>
        public List<PlanDetailDto> PlanDetails { get; set; }

        /// <summary>
        /// 登记地点
        /// </summary>
        public string RegistrationPlace { get; set; }


        /// <summary>
        /// 是否为相邻区间
        /// </summary>
        public bool IsAdjacent { get; set; }

        /// <summary>
        /// 终止站点关联线路类型
        /// </summary>
        public RelateRailwayType EndStationRelateRailwayType { get; set; }

        /// <summary>
        /// 是否上道
        /// </summary>
        public bool IsOnRoad { get; set; }

        /// <summary>
        /// 终止站点Id
        /// </summary>
        public Guid? EndStationId { get; set; }
        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }

        public SkylightPlanDetailDto() { }

        public SkylightPlanDetailDto(Guid id)
        {
            Id = id;
        }

        public string SignOrganization { get; set; }

        public string FirstTrial { get; set; }
        public string WorkOrgAndDutyPerson { get; set; }

        /// <summary>
        /// 备注(维修作业）
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; set; }

       
    }
}
