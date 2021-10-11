using SnAbp.Emerg.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Emerg.Dtos
{
    public class FaultCreateDto
    {
        /// <summary>
        /// 所属组织ID
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 所属线路ID
        /// </summary>
        public Guid RailwayId { get; set; }

        /// <summary>
        /// 所属站点ID
        /// </summary>
        public Guid StationId { get; set; }

        /// <summary>
        /// 关联构件
        /// </summary>
        public List<FaultRltComponentCategoryCreateDto> FaultRltComponentCategories { get; set; } = new List<FaultRltComponentCategoryCreateDto>();

        /// <summary>
        /// 关联设备
        /// </summary>
        public List<FaultRltEquipmentCreateDto> FaultRltEquipments { get; set; } = new List<FaultRltEquipmentCreateDto>();

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentNames { get; set; }

        /// <summary>
        /// 故障概述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 故障等级ID
        /// </summary>
        public Guid LevelId { get; set; }

        /// <summary>
        /// 故障详情
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 监测异常
        /// </summary>
        public string Abnormal { get; set; }

        /// <summary>
        /// 原因分类ID
        /// </summary>
        public Guid ReasonTypeId { get; set; }

        /// <summary>
        /// 原因分析
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 天气详情
        /// </summary>
        public string WeatherDetail { get; set; }

        /// <summary>
        /// 最高气温 
        /// </summary>
        public float? TemperatureMax { get; set; }

        /// <summary>
        /// 最低气温
        /// </summary>
        public float? TemperatureMin { get; set; }

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
        /// 来源
        /// </summary>
        public FaultSource Source { get; set; } = FaultSource.System;

        /// <summary>
        /// 应急预案ID
        /// </summary>
        public Guid? EmergPlanRecordId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public FaultState State { get; set; }

        /// <summary>
        /// 登记/故障时间
        /// </summary>
        public DateTime CheckInTime { get; set; }

        /// <summary>
        /// 销记时间
        /// </summary>
        public DateTime? CheckOutTime { get; set; }

        /// <summary>
        /// 登记人
        /// </summary>
        public Guid? CheckInUserId { get; set; }

        /// <summary>
        /// 销记人
        /// </summary>
        public Guid? CheckOutUserId { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        public Guid? SubmitUserId { get; set; }
    }
}
