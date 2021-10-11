using SnAbp.Identity;
using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 设备上下道（安装、更改、拆除记录表）
    /// </summary>
    public class EquipmentServiceRecord : FullAuditedEntity<Guid>
    {
        protected EquipmentServiceRecord() { }
        public EquipmentServiceRecord(Guid id) { Id = id; }

        /// <summary>
        /// 设备 Id
        /// </summary>
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        /// <summary>
        /// 库存设备 Id
        /// </summary>
        public Guid StoreEquipmentId { get; set; }
        public StoreEquipment StoreEquipment { get; set; }

        /// <summary>
        /// 记录类型
        /// </summary>
        public EquipmentServiceRecordType Type { get; set; }

        /// <summary>
        /// 关联人员Id
        /// </summary>
        public Guid? UserId { get; set; }
        public IdentityUser User { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }
    }
}
