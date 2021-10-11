using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 物资验收关联文件实体
    /// </summary>
    public class MaterialAcceptanceRltFile : AuditedEntity<Guid>
    {
        public virtual Guid MaterialAcceptanceId { get; set; }
        public virtual MaterialAcceptance MaterialAcceptance { get; set; }

        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }
        public MaterialAcceptanceRltFile() { }
        public MaterialAcceptanceRltFile(Guid id)
        {
            Id = id;
        }
    }
}
