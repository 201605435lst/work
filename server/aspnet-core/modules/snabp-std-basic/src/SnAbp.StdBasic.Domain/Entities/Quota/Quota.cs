/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Quota
*******类 名 称： Quota
*******类 说 明： 定额表实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 9:53:34
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
    /// 定额表实体
    /// </summary>
    public class Quota : Entity<Guid>
    {
        public Quota(Guid id) => this.Id = id;
        /// <summary>
        /// 定额分类
        /// </summary>
        public virtual QuotaCategory QuotaCategory { get; set; }
        public virtual Guid QuotaCategoryId { get; set; }
        /// <summary>
        /// 定额名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 定额编码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public virtual string Unit { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public virtual decimal Weight { get; set; }
        /// <summary>
        /// 人工费
        /// </summary>
        public virtual decimal LaborCost { get; set; }
        /// <summary>
        /// 材料费
        /// </summary>
        public virtual decimal MaterialCost { get; set; }
        /// <summary>
        /// 机械使用费
        /// </summary>
        public virtual decimal MachineCost { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
