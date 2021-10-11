using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 物资验收关联二维码实体
    /// </summary>
    public class MaterialAcceptanceRltQRCode : Entity<Guid>
    {
        /// <summary>
        /// 关联验收单
        /// </summary>
        public virtual Guid MaterialAcceptanceId { get; set; }
        public virtual MaterialAcceptance MaterialAcceptance { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
        protected MaterialAcceptanceRltQRCode() { }
        public MaterialAcceptanceRltQRCode(Guid id)
        {
            Id = id;
        }
       
    }
}
