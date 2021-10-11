using SnAbp.MultiProject.MultiProject;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
/// <summary>
/// 档案分类
/// </summary>
namespace SnAbp.Project.Entities
{
    public class ArchivesCategory : FullAuditedEntity<Guid>, IGuidKeyTree<ArchivesCategory>
    {
        /// <summary>
        /// 上级分类
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        public virtual ArchivesCategory Parent { get; set; }

        public List<ArchivesCategory> Children { get; set; }
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

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        public ArchivesCategory()
        {

        }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public ArchivesCategory(Guid id)
        {
            Id = id;
        }
    }
}
