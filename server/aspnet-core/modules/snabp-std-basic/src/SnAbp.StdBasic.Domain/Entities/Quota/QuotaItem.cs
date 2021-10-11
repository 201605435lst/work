/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Quota
*******类 名 称： QuotaItem
*******类 说 明： 清单表实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:07:29
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
    /// 清单表实体
    /// </summary>
    public class QuotaItem : Entity<Guid>
    {
        public QuotaItem(Guid id) => this.Id = id;

        public virtual Guid QuotaId { get; set; }
        /// <summary>
        /// 定额
        /// </summary>
        public virtual Quota Quota { get; set; }
        public virtual Guid BasePriceId { get; set; }
        /// <summary>
        /// 基价
        /// </summary>
        public virtual BasePrice BasePrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal Number { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
