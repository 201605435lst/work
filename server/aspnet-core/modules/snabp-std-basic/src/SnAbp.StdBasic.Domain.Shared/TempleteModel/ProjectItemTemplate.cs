using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic
{
    public class ProjectItemTemplate
    {
        public int Index { get; set; }
       
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
