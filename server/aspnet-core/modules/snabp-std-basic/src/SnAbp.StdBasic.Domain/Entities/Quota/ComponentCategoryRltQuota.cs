/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Quota
*******类 名 称： ComponentCategoryRltQuota
*******类 说 明： 构件分类与定额关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:10:55
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
    /// 构件分类与定额关联表
    /// </summary>
    public class ComponentCategoryRltQuota : Entity<Guid>
    {
        public ComponentCategoryRltQuota(Guid id) => this.Id = id;
        public ComponentCategoryRltQuota(Guid componentCategoryId, Guid quotaId)
        {
            this.ComponentCategoryId = componentCategoryId;
            this.QuotaId = quotaId;

        }
        public virtual ComponentCategory ComponentCategory { get; set; }
        public virtual Guid ComponentCategoryId { get; set; }

        public virtual Quota Quota { get; set; }
        public virtual Guid QuotaId { get; set; }

        public override object[] GetKeys() => new object[] { ComponentCategoryId, QuotaId };
    }
}
