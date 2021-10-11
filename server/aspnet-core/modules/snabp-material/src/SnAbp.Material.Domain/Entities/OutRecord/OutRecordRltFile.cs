using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 出库关联资料
    /// </summary>
    public class OutRecordRltFile : Entity<Guid>
    {
        /// <summary>
        /// 出库记录
        /// </summary>
        public virtual Guid OutRecordId { get; set; }
        public virtual OutRecord OutRecord { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        protected OutRecordRltFile() { }
        public OutRecordRltFile(Guid id)
        {
            Id = id;
        }
    }
}
