using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 维修计划关联文件 (方案)
    /// </summary>
    public class MaintenanceWorkRltFile : Entity<Guid>
    {
        /// <summary>
        /// 维修作业
        /// </summary>
        public Guid MaintenanceWorkId { get; set; }
        public virtual MaintenanceWork MaintenanceWork { get; set; }

        /// <summary>
        /// 文件(方案)
        /// </summary>
        public Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        /// <summary>
        /// 文件关联的文件(方案封面)
        /// </summary>
        public Guid? RelateFileId { get; set; }
        public virtual File.Entities.File RelateFile { get; set; }

        //方案封面名称
        public string SchemeCoverName { get; set; }

        public MaintenanceWorkRltFile() { }

        public MaintenanceWorkRltFile(Guid id) { Id = id; }

    }
}
