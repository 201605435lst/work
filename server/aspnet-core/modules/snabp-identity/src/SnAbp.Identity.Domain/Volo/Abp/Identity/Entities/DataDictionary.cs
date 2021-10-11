/**********************************************************************
*******命名空间： Volo.Abp.Identity.Entities
*******类 名 称： DataDictionary
*******类 说 明： 数字字典表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/18 11:27:19
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Identity
{
    public class DataDictionary : FullAuditedEntity<Guid>
    {
        public DataDictionary(Guid id)
        {
            Id = id;
        }
        protected DataDictionary() { }

        public void SetId(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 字典值
        /// </summary>
        [MaxLength(100)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public virtual Guid? ParentId { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        [MaxLength(100)]
        public virtual string Key { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Order { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否系统使用
        /// </summary>
        public virtual bool IsStatic { get; set; }

        public virtual DataDictionary Parent { get; set; }

        public virtual List<DataDictionary> Children { get; set; }

    }
}
