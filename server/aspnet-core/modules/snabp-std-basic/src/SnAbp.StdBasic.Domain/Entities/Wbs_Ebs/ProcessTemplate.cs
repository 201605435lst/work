/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Wbs_Ebs
*******类 名 称： ProcessTemplate
*******类 说 明： 工序模板 表实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:26:07
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SnAbp.StdBasic.Enums;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 工序模板 表实体
    /// </summary>
    public class ProcessTemplate : Entity<Guid>, IGuidKeyTree<ProcessTemplate>
    {
        public ProcessTemplate(Guid id) => this.Id = id;
        public virtual Guid? ParentId { get; set; }
        /// <summary>
        /// 前置任务id
        /// </summary>
        public virtual Guid? PrepositionId { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        [CanBeNull]
        public virtual ProcessTemplate Parent { get; set; }
        public virtual List<ProcessTemplate> Children { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 工作项单位
        /// </summary>
        public virtual string Unit { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 工序类别
        /// </summary>
        public virtual ProcessTypeEnum Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 工期
        /// </summary>
        public virtual decimal Duration { get; set; }
        /// <summary>
        /// 工期单位
        /// </summary>
        public virtual ServiceLifeUnit DurationUnit { get; set; }
    }
}
