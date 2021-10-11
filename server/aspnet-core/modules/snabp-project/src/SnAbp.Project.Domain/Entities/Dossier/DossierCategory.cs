using SnAbp.MultiProject.MultiProject;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
/// <summary>
/// 卷宗分类
/// </summary>
namespace SnAbp.Project.Entities
{
    public class DossierCategory:FullAuditedEntity<Guid>,IGuidKeyTree<DossierCategory>
    {
        /// <summary>
        /// 上级分类
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        public virtual DossierCategory Parent { get; set; }

        public List<DossierCategory> Children { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        public DossierCategory()
        {

        }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public DossierCategory(Guid id)
        {
            Id = id;
        }
    }
}
