using SnAbp.File.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 入库关联资料
    /// </summary>
    public class EntryRecordRltFileDto : EntityDto<Guid>
    {
        /// <summary>
        /// 入库记录
        /// </summary>
        public virtual Guid EntryRecordId { get; set; }
        public virtual EntryRecordDto EntryRecord { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual FileSimpleDto File { get; set; }

        protected EntryRecordRltFileDto() { }
        public EntryRecordRltFileDto(Guid id)
        {
            Id = id;
        }
    }
}
