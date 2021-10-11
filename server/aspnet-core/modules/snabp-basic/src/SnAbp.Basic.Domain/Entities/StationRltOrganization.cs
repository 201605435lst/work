using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Basic.Entities
{
    /// <summary>
    /// 车站维护班组
    /// </summary>
    public class StationRltOrganization : Entity<Guid>
    {
         public Guid? ProjectTagId { get; set; }

        /// <summary>
        /// 站点Guid
        /// </summary>
        [ForeignKey("Station")]
        public Guid StationId { get; set; }
        public Station Station { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }

        /// <summary>
        /// 是否备用
        /// </summary>
        public bool IsBackUp { get; set; }

        protected StationRltOrganization() { }
        public StationRltOrganization(Guid id)
        {
            Id = id;
        }
    }
}
