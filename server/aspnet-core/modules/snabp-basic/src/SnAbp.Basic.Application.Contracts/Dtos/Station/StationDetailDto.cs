using SnAbp.Identity;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class StationDetailDto : EntityDto<Guid>
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型  0车站  1区间
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 所属线路信息
        /// </summary>
        public List<BelongRailwayInfo> BelongRailways { get; set; } = new List<BelongRailwayInfo>();

        /// <summary>
        /// 维护班组
        /// </summary>
        public List<OrganizationDto> RepairTeams { get; set; } = new List<OrganizationDto>();

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class BelongRailwayInfo
    {
        public BelongRailwayInfo(RailwayDto railway, int kmmark)
        {
            Railway = railway;
            KMMark = kmmark;
        }

        public RailwayDto Railway { get; set; }
        public int KMMark { get; set; }
    }
}
