/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Quota
*******类 名 称： ComputerCode
*******类 说 明： 电算代号 实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 9:57:59
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.StdBasic.Enums;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 电算代号
    /// </summary>
    public class ComputerCode:Entity<Guid>
    {
        public ComputerCode(Guid id) => this.Id = id;
        /// <summary>
        /// 电算代号
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 名称及规格
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public virtual string Unit { get; set; }
        /// <summary>
        /// 电算代号类型
        /// </summary>
        public virtual ComputerCodeType Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 单位重量
        /// </summary>
        public virtual decimal Weight { get; set; }
    }
}
