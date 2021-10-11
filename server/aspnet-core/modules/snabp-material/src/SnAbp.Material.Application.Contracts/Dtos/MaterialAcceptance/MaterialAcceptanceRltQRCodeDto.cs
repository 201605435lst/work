using SnAbp.StdBasic.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class MaterialAcceptanceRltQRCodeDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联验收单
        /// </summary>
        public virtual Guid MaterialAcceptanceId { get; set; }
        public virtual MaterialAcceptanceDto MaterialAcceptance { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
        public ComponentCategoryDto ComponentCategory { get; set; }
        public Guid ComponentCategoryId { get; set; }

        protected MaterialAcceptanceRltQRCodeDto() { }
        public MaterialAcceptanceRltQRCodeDto(Guid id)
        {
            Id = id;
        }
    }
}
