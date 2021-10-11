/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Wbs_Ebs
*******类 名 称： ProjectItem
*******类 说 明： 工程工项
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:36:06
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;

using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 工程工项
    /// </summary>
    public class ProjectItem : Entity<Guid>
    {
        public ProjectItem(Guid id) => this.Id = id;
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
