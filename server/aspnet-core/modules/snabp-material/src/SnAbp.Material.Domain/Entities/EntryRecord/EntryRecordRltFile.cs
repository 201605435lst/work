using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 入库关联资料
    /// </summary>
    public class EntryRecordRltFile : Entity<Guid>
    {
        /// <summary>
        /// 入库记录
        /// </summary>
        public virtual Guid EntryRecordId { get; set; }
        public virtual EntryRecord EntryRecord { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        protected EntryRecordRltFile() { }
        public EntryRecordRltFile(Guid id)
        {
            Id = id;
        }
    }
}
