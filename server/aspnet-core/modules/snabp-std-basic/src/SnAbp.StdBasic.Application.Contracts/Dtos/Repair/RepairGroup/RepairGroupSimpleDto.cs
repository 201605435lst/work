using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 维护分组
    /// </summary>
    public class RepairGroupSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public Guid? ParentId { get; set; }
        public virtual RepairGroupDto Parent { get; set; }
        public List<RepairGroupDto> Children { get; set; }

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
