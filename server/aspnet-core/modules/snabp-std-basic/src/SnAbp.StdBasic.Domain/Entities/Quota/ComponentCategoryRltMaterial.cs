/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Quota
*******类 名 称： ComponentCategoryRltMaterial
*******类 说 明： 构件分类与材料关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:15:39
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
    /// 构件分类与材料关联表
    /// </summary>
    public class ComponentCategoryRltMaterial : Entity<Guid>
    {
        public ComponentCategoryRltMaterial(Guid id) => this.Id = id;
        public ComponentCategoryRltMaterial(Guid componentCategoryId, Guid computerCodeId)
        {
            this.ComponentCategoryId = componentCategoryId;
            this.ComputerCodeId = computerCodeId;
        }

        public virtual Guid ComponentCategoryId { get; set; }
        public virtual ComponentCategory ComponentCategory { get; set; }

        public virtual Guid ComputerCodeId { get; set; }
        public virtual ComputerCode ComputerCode { get; set; }
        public override object[] GetKeys() => new object[] { ComponentCategoryId, ComputerCodeId };
    }
}
