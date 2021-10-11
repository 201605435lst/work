using SnAbp.Emerg.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class FaultSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 所属组织ID
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// 所属线别ID
        /// </summary>
        public Guid? RailwayId { get; set; }


        /// <summary>
        /// 所属站点ID
        /// </summary>
        public Guid? StationId { get; set; }

        /// <summary>
        /// 构件id集合
        /// </summary>
        public List<Guid> ComponentCategoryIds { get; set; } = new List<Guid>();


        /// <summary>
        /// 设备名称
        /// </summary>
        public List<Guid> EquipmentIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public FaultState? State { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public EmergPlanRecordRltMemberGroup? Group { get; set; }

        /// <summary>
        /// 故障概述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 故障详情
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 监测异常
        /// </summary>
        public string Abnormal { get; set; }

        /// <summary>
        /// 原因分析
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 天气详情
        /// </summary>
        public string WeatherDetail { get; set; }

        /// <summary>
        /// 处理过程
        /// </summary>
        public string DisposeProcess { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string DisposePersons { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 3D故障应急集成查询
        /// </summary>
        public int PendingAndUnchecked { get; set; }
    }
}