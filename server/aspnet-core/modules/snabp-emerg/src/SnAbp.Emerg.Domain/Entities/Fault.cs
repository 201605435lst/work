using SnAbp.Basic.Entities;
using SnAbp.Emerg.Enums;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Emerg.Entities
{
    public class Fault : FullAuditedEntity<Guid>
    {
        protected Fault() { }
        public Fault(Guid id) { Id = id; }


        /// <summary>
        /// 所属组织
        /// </summary>
        [Required]
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// 所属线路
        /// </summary>
        [Required]
        public Guid RailwayId { get; set; }
        public virtual Railway Railway { get; set; }

        /// <summary>
        /// 所属站点
        /// </summary>
        [Required]
        public Guid StationId { get; set; }
        public virtual Station Station { get; set; }

        [InverseProperty("Fault")]
        /// <summary>
        /// 构件分类
        /// </summary>
        public List<FaultRltComponentCategory> FaultRltComponentCategories { get; set; }

        [InverseProperty("Fault")]
        /// <summary>
        /// 关联设备
        /// </summary>
        public List<FaultRltEquipment> FaultRltEquipments { get; set; }

        [MaxLength(120)]
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentNames { get; set; }

        /// <summary>
        /// 故障概述  /   
        /// </summary>
        [MaxLength(120)]
        public string? Summary { get; set; }

        /// <summary>
        /// 故障等级
        /// </summary>
        public Guid LevelId { get; set; }
        public virtual DataDictionary Level { get; set; }

        /// <summary>
        /// 故障详情
        /// </summary>
        [MaxLength(1000)]
        public string? Content { get; set; }

        /// <summary>
        /// 监测异常
        /// </summary>
        [MaxLength(1000)]
        public string? Abnormal { get; set; }

        /// <summary>
        /// 原因分类
        /// </summary>
        public Guid ReasonTypeId { get; set; }
        public virtual DataDictionary ReasonType { get; set; }

        /// <summary>
        /// 原因分析
        /// </summary>
        [MaxLength(1000)]
        public string? Reason { get; set; }

        /// <summary>
        /// 天气详情
        /// </summary>
        [MaxLength(120)]
        public string? WeatherDetail { get; set; }

        /// <summary>
        /// 最高气温 // 温差
        /// </summary>
        public float? TemperatureMax { get; set; }

        /// <summary>
        /// 最低气温
        /// </summary>
        public float? TemperatureMin { get; set; }

        /// <summary>
        /// 处理过程
        /// </summary>
        [MaxLength(5000)]
        public string? DisposeProcess { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        [MaxLength(500)]
        public string? DisposePersons { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        public string? Remark { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public FaultSource Source { get; set; } = FaultSource.System;

        /// <summary>
        /// 应急预案
        /// </summary>
        public Guid? EmergPlanRecordId { get; set; }
        public virtual EmergPlanRecord EmergPlanRecord { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public FaultState State { get; set; }

        /// <summary>
        /// 登记时间
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
        public virtual IdentityUser CheckInUser { get; set; }

        /// <summary>
        /// 销记人
        /// </summary>
        public Guid? CheckOutUserId { get; set; }
        public virtual IdentityUser CheckOutUser { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        public Guid? SubmitUserId { get; set; }
        public virtual IdentityUser SubmitUser { get; set; }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
