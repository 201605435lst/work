using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Dtos
{
    /// <summary>
    /// 机房级联选择结点
    /// </summary>
    public class InstallationSiteCascaderDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 类型 线 0 站1 组织机构2 机房 3 其他4
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 子项
        /// </summary>
        public List<InstallationSiteCascaderDto> Children { get; set; } = new List<InstallationSiteCascaderDto>();
    }
}
