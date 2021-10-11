using SnAbp.MultiProject.MultiProject;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnAbp.Bpm.Entities
{
    [NotMapped]
    public class FlowNodeFormItemPermisstion
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 是否可查看
        /// </summary>
        public bool View { get; set; }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool Edit { get; set; }

        /// <summary>
        /// 是否显示简报
        /// </summary>
        public bool Info { get; set; }
    }
}