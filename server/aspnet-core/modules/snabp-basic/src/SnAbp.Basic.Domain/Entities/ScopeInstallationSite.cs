using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Basic.Enums;
using SnAbp.MultiProject.MultiProject;

namespace SnAbp.Basic.Entities
{
    public class ScopeInstallationSite
    {

        /// <summary>
        /// 组织|线路|站点|安装位置Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public virtual Guid? ParentId { get; set; }
        public virtual ScopeInstallationSite Parent { get; set; }

        /// <summary>
        /// 子级
        /// </summary>
        public virtual List<ScopeInstallationSite> Children { get; set; }

        /// <summary>
        /// Id对应的name值
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ScopeType Type { get; set; }

        public ScopeInstallationSite()
        {
            
        }
    }
}
