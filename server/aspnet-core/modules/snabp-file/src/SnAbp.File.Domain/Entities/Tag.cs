/***********************[***********************************************
*******命名空间： SnAbp.File.Entities
*******类 名 称： Tag
*******类 说 明： 资源标签实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/9 8:39:20
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.File.Entities
{
    /// <summary>
    ///     资源标签实体，继承 <see cref="AggregateRoot" />  聚合根类，使用了扩展属性字段
    /// </summary>
    public class Tag : AggregateRoot<Guid>
    {
        public Tag()
        {
        }

        public Tag(Guid id)
        {
            Id = id;
        }

        /// <summary>
        ///     组织id
        /// </summary>
        public virtual Guid OrganizationId { get; set; }

        /// <summary>
        ///     标签名称
        /// </summary>
        public virtual string Name { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}