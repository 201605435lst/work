/**********************************************************************
*******命名空间： SnAbp.Technology.Entities.Material
*******类 名 称： Material
*******类 说 明： 物资材料表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 11:14:24 AM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Technology.Entities
{
    /// <summary>
    /// 物资材料表
    /// </summary>
    public class Material:Entity<Guid>
    {
        public Material(Guid id) => Id = id;

        public void SetId(Guid id) => Id = id;
        /// <summary>
        /// 材料名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属材料类别（数据字典）
        /// </summary>
        public virtual Guid? TypeId { get; set; }
        public virtual DataDictionary Type { get; set; }
        /// <summary>
        /// 材料规格/型号
        /// </summary>
        public string Spec { get; set; }

        // 产品分类编码
        public Guid? ProductCategoryId { get; set; }
        /// <summary>
        /// 材料型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 材料单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 材料价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
