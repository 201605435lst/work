using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Basic.Entities
{
    public class RailwayRltOrganization : Entity<Guid>
    {
        /// <summary>
        /// 线路id
        /// </summary>
        public Guid? RailwayId { get; set; }
        public virtual Railway Railway { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// 下行区段起始公里标
        /// </summary>
        public int DownLinkStartKM { get; set; }

        /// <summary>
        /// 下行区段终止公里标
        /// </summary>
        public int DownLinkEndKM { get; set; }

        /// <summary>
        /// 上行区段起始公里标
        /// </summary>
        public int UpLinkStartKM { get; set; }

        /// <summary>
        /// 上行区段终止公里标
        /// </summary>
        public int UpLinkEndKM { get; set; }
         public Guid? ProjectTagId { get; set; }
        protected RailwayRltOrganization() { }
        public RailwayRltOrganization(Guid id)
        {
            Id = id;
        }
    }
}
