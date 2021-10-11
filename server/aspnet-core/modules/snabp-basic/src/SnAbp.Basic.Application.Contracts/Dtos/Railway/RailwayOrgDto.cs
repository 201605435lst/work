using SnAbp.Basic.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class RailwayOrgDto : EntityDto<Guid>
    {
        /// <summary>
        /// 线路id
        /// </summary>
        public Guid? RailwayId { get; set; }
        public RailwayDto Railway { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid? OrganizationId { get; set; }
        public OrganizationDto Organization { get; set; }

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
    }
}
