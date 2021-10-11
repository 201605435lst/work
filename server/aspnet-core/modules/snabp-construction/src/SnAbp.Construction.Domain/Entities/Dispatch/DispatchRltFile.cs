using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    /// 派工单关联文件
    /// </summary>
    public class DispatchRltFile : Entity<Guid>
    {
        public DispatchRltFile(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual Dispatch Dispatch { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }
    }
}
