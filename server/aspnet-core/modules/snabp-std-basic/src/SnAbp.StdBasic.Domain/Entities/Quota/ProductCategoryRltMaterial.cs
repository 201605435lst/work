/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Quota
*******类 名 称： ProductCategoryRltMaterial
*******类 说 明： 产品分类与材料关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:18:41
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
    /// 产品分类与材料关联表
    /// </summary>
    public class ProductCategoryRltMaterial : Entity<Guid>
    {
        public ProductCategoryRltMaterial(Guid id) => this.Id = id;
        public ProductCategoryRltMaterial(Guid productCategoryId, Guid computerCodeId)
        {
            this.ProductCategoryId = productCategoryId;
            this.ComputerCodeId = computerCodeId;
        }

        public virtual Guid ProductCategoryId { get; set; }
        public virtual Guid ComputerCodeId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ComputerCode ComputerCode { get; set; }

        public override object[] GetKeys() => new object[] { ProductCategoryId, ComputerCodeId };
    }
}
