using SnAbp.Basic.Enums;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Basic.Entities
{
    public class Railway: FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 线路名称
        /// </summary>
        [StringLength(50)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 类型  枚举  0单线 1复线
        /// </summary>
        public RailwayType Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Description("备注")]
        public string Remark { get; set; }

        public virtual List<RailwayRltOrganization> RailwayRltOrganizations { get; set; }

         public Guid? ProjectTagId { get; set; }

        protected Railway() { }
        public Railway(Guid id)
        {
            Id = id;
        }
    }
}
