using System;

namespace SnAbp.Material.Dtos
{
    public class MaterialAcceptanceRltQRCodeCreateDto
    {
        /// <summary>
        /// 关联验收单
        /// </summary>
        public virtual Guid MaterialAcceptanceId { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
    }
}
