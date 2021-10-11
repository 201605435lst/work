using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Alarm.Entities
{
    // 第三方设备 ids 绑定
    public class AlarmEquipmentIdBind : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        public AlarmEquipmentIdBind() { }
        public AlarmEquipmentIdBind(Guid id) { Id = id; }


        // 设备 id
        public Guid EquipmentId { get; set; }

        // 第三方设备 id
        public string EquipmentThirdIds { get; set; }
    }
}

