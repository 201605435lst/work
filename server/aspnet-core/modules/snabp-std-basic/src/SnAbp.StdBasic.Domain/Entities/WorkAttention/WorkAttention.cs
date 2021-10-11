using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    public class WorkAttention : AuditedEntity<Guid>
    {
        /// <summary>
        /// 类别Id
        /// </summary>
        public Guid? TypeId { get; set; }
        public virtual WorkAttention Type { get; set; }

        /// <summary>
        /// 数据是否是“类别”类型
        /// </summary>
        public bool IsType { get; set; }

        /// <summary>
        /// 注意事项 或 类别名称
        /// </summary>
        public string Content { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        public WorkAttention() { }
        public WorkAttention(Guid id)
        {
            Id = id;
        }
    }
}
