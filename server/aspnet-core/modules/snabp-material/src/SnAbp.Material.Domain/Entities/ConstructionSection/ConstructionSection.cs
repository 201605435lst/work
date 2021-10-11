using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    public class ConstructionSection : AuditedEntity<Guid>
    {
         /// <summary>
        /// 施工区段名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 起始锚段
        /// </summary>
        public string StartSegment { get; set; }
        /// <summary>
        /// 终止锚段
        /// </summary>
        public string EndSegment { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        public void SetId(Guid id)
        {
            Id = id;
        }
        public ConstructionSection() { }
        public ConstructionSection(Guid id)
        {
            Id = id;
        }
    }
}
