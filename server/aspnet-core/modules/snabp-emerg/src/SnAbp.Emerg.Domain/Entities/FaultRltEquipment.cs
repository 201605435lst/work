using SnAbp.MultiProject.MultiProject;
using SnAbp.Resource.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Emerg.Entities
{
    public class FaultRltEquipment : Entity<Guid>
    {
        protected FaultRltEquipment() { }
        public FaultRltEquipment(Guid id) { Id = id; }

        /// <summary>
        /// 故障
        /// </summary>
        public Guid FaultId { get; set; }
        public virtual Fault Fault { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public Guid EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
