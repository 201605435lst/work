using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class RailwayOrgCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

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
