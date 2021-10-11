using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using SnAbp.Utils.TreeHelper;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 维护分组
    /// </summary>
    public class RepairGroup : Entity<Guid>, IGuidKeyTree<RepairGroup>
    {
        protected RepairGroup() { }
        public RepairGroup(Guid id) { Id = id; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public Guid? ParentId { get; set; }
        public virtual RepairGroup Parent { get; set; }
        public List<RepairGroup> Children { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
