using SnAbp.Emerg.Enums;
using System;
using System.Collections.Generic;
using SnAbp.Basic.Dtos;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class FaultSimpleDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 登记/故障时间
        /// </summary>
        public DateTime CheckInTime { get; set; }

        /// <summary>
        /// 关联构件
        /// </summary>
        public List<FaultRltComponentCategorySimpleDto> FaultRltComponentCategories { get; set; } = new List<FaultRltComponentCategorySimpleDto>();

        /// <summary>
        /// 关联设备
        /// </summary>
        public List<FaultRltEquipmentSimpleDto> FaultRltEquipments { get; set; } = new List<FaultRltEquipmentSimpleDto>();

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentNames { get; set; }

        /// <summary>
        /// 监测异常
        /// </summary>
        public string? Abnormal { get; set; }

        /// <summary>
        /// 故障详情
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 所属站点ID
        /// </summary>
        public Guid StationId { get; set; }
        public StationDto Station { get; set; }

        /// <summary>
        /// 故障概述
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// 故障等级ID
        /// </summary>
        public Guid LevelId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public FaultState State { get; set; }

        /// <summary>
        /// 应急预案ID
        /// </summary>
        public Guid? EmergPlanRecordId { get; set; }

        /// <summary>
        /// 处理过程
        /// </summary>
        public string? DisposeProcess { get; set; }
    }
}