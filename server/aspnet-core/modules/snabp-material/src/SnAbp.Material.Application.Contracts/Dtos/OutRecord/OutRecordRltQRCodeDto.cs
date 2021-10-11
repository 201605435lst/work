using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 出库关联二维码
    /// </summary>
    public class OutRecordRltQRCodeDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联出库记录
        /// </summary>
        public virtual Guid OutRecordId { get; set; }
        public virtual OutRecordDto OutRecord { get; set; }

        /// <summary>
        /// 关联构件分类
        /// </summary>
        public virtual Guid ComponentCategoryId { get; set; }
        public virtual ComponentCategoryDto ComponentCategory { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
        protected OutRecordRltQRCodeDto() { }
        public OutRecordRltQRCodeDto(Guid id)
        {
            Id = id;
        }
    }
}
