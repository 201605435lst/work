using SnAbp.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Project.Dtos
{
    public class ArchivesCategoryDto : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 上级分类
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        public virtual ArchivesCategory Parent { get; set; }

        public List<ArchivesCategoryDto> Children { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsEncrypt { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
