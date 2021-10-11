using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 出库关联二维码
    /// </summary>
    public class OutRecordRltQRCode : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 关联出库记录
        /// </summary>
        public virtual Guid OutRecordId { get; set; }
        public virtual OutRecord OutRecord { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
        protected OutRecordRltQRCode() { }
        public OutRecordRltQRCode(Guid id)
        {
            Id = id;
        }
    }
}
