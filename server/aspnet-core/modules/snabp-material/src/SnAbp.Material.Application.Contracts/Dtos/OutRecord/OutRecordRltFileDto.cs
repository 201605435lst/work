using SnAbp.File.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 出库关联资料
    /// </summary>
    public class OutRecordRltFileDto : EntityDto<Guid>
    {
        /// <summary>
        /// 出库记录
        /// </summary>
        public virtual Guid OutRecordId { get; set; }
        public virtual OutRecordDto OutRecord { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual FileSimpleDto File { get; set; }

        protected OutRecordRltFileDto() { }
        public OutRecordRltFileDto(Guid id)
        {
            Id = id;
        }
    }
}
