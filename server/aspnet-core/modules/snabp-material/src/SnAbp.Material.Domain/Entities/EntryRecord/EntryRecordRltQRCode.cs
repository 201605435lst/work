using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 入库关联二维码
    /// </summary>
    public class EntryRecordRltQRCode : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 关联入库记录
        /// </summary>
        public virtual Guid EntryRecordId { get; set; }
        public virtual EntryRecord EntryRecord { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
        protected EntryRecordRltQRCode() { }
        public EntryRecordRltQRCode(Guid id)
        {
            Id = id;
        }
    }
}
