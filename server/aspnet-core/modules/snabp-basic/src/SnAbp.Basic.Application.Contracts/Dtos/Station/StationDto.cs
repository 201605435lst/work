using SnAbp.Basic.Dtos;
using SnAbp.Basic;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class StationDto : EntityDto<Guid>
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 类型  0车站  1区间
        /// </summary>
        public byte Type { get; set; }
        public string TypeStr
        {
            get
            {
                var res = "";
                switch (Type)
                {
                    case 0:
                        res = "车站";
                        break;
                    case 1:
                        res = "区间";
                        break;
                }
                return res;
            }
        }

        /// <summary>
        /// 区间起始站
        /// </summary>
        public Guid? SectionStartStationId { get; set; }
        /// <summary>
        /// 区间终止站
        /// </summary>
        public Guid? SectionEndStationId { get; set; }

        /// <summary>
        /// 公里标
        /// </summary>
        public int KMMark { get; set; }


        /// <summary>
        /// 维护班组
        /// </summary>
        //public List<OrganizationDto> RepairTeam { get; set; } = new List<OrganizationDto>();

        /// <summary>
        /// 所属线路
        /// </summary>
        //public List<RailwayDto> BelongRailway { get; set; } = new List<RailwayDto>();

        /// <summary>
        /// 维护班组展示str
        /// </summary>
        //public string RepairTeamStr { get; set; }

        /// <summary>
        /// 维护班组父级显示Str
        /// </summary>
        //public string RepairTeamParentStr { get; set; }

        /// <summary>
        /// 站点排序
        /// </summary>
        public int PassOrder { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
