using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 入库关联二维码
    /// </summary>
    public class EntryRecordRltQRCodeDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联入库记录
        /// </summary>
        public virtual Guid EntryRecordId { get; set; }
        public virtual EntryRecordDto EntryRecord { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
        protected EntryRecordRltQRCodeDto() { }
        public EntryRecordRltQRCodeDto(Guid id)
        {
            Id = id;
        }
    }
}
