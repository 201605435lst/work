using SnAbp.Identity;
using SnAbp.Resource.Entities;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Dtos
{
    /// <summary>
    /// 设备检测单
    /// </summary>
    public class StoreEquipmentTestDto : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 检测机构
        /// </summary>
        public Guid? OrganizationId { get; set; }
        public Organization Organization { get; set; }

        /// <summary>
        /// 检测机构名称
        /// </summary>
        public string? OrganizationName { get; set; }

        /// <summary>
        /// 检测地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 检测人 Id
        /// </summary>
        public Guid? TesterId { get; set; }
        //public IdentityUser Tester { get; set; }

        /// <summary>
        /// 检测人姓名
        /// </summary>
        public string TesterName { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool Passed { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 检测内容（富文本）
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 关联设备
        /// </summary>
        public List<StoreEquipmentTestRltEquipment> StoreEquipmentTestRltEquipments { get; set; }
    }
}
