using System;
using Volo.Abp.Domain.Entities.Auditing;
using SnAbpFile = SnAbp.File.Entities.File;

namespace SnAbp.Resource.Entities
{
    public class EquipmentRltFile: FullAuditedEntity<Guid>
    {
        protected EquipmentRltFile() { }
        public EquipmentRltFile(Guid id) { Id = id; }

        /// <summary>
        /// 设备
        /// </summary>
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public Guid FileId { get; set; }
        public SnAbpFile File { get; set; }
    }
}
