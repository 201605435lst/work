/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Quota
*******类 名 称： BasePrice
*******类 说 明： 基价
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:03:42
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using SnAbp.Common.Entities;
using SnAbp.Identity;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 基价表实体
    /// </summary>
    public class BasePrice : Entity<Guid>
    {
        public BasePrice(Guid id) => this.Id = id;

        public virtual Guid ComputerCodeId { get; set; }
        /// <summary>
        /// 电算代号
        /// </summary>
        public virtual ComputerCode ComputerCode { get; set; }
        /// <summary>
        /// 基础单价
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 标准编号数据字典
        /// </summary>
        public virtual DataDictionary StandardCode { get; set; }

        /// <summary>
        ///标准编号 Code 来源于数据字典
        /// </summary>
        public virtual DataDictionary Standard { get; set; }
        public virtual Guid StandardCodeId { get; set; }
        /// <summary>
        /// 行政区划
        /// </summary>
        public virtual int AreaId { get; set; }
        public virtual Area Area { get; set; }
    }
}
