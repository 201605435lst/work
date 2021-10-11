using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    public class MaterialOfBillRltAccessory : AuditedEntity<Guid>
    {
        public MaterialOfBillRltAccessory(Guid id) => Id = id;

        public virtual MaterialOfBill MaterialOfBill { get; set; }
        public virtual Guid MaterialOfBillId { get; set; }

        public File.Entities.File File { get; set; }
        public virtual Guid? FileId { get; set; }
    }
}
