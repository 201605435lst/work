using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 工程工项
    /// </summary>
    public class ProjectItemCreateDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
