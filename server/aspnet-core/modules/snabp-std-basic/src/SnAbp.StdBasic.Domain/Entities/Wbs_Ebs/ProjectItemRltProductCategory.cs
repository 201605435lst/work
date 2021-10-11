/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Wbs_Ebs
*******类 名 称： ProjectItemRltProductCategory
*******类 说 明： 工程工项与产品分类关联
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:46:11
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 工程工项与产品分类关联
    /// </summary>
    public class ProjectItemRltProductCategory : Entity<Guid>
    {
        public ProjectItemRltProductCategory(Guid id) => this.Id = id;
        public ProjectItemRltProductCategory(Guid projectItemId, Guid productCategoryId)
        {
            this.ProjectItemId = projectItemId;
            this.ProductCategoryId = productCategoryId;
        }
        public virtual Guid ProjectItemId { get; set; }
        public virtual Guid ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }
        public override object[] GetKeys() => new object[] { ProjectItemId, ProductCategoryId };

    }
}
