using SnAbp.Basic.Dtos;
using SnAbp.Basic.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class RailwaySimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 线路名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型  枚举  0单线 1复线
        /// </summary>
        public RailwayType Type { get; set; }

        /// <summary>
        /// 类型 字符串
        /// </summary>
        public string TypeStr
        {
            get
            {
                var res = "";
                switch (Type)
                {
                    case  RailwayType.MONGLINE:
                        res = "单线";
                        break;
                    case RailwayType.COMPLEXLINE:
                        res = "复线";
                        break;
                }
                return res;
            }
        }

        /// <summary>
        /// 所属单位字符串
        /// </summary>
        public string BelongOrgsStr { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 包含站点信息  单线 或复线的上行
        /// </summary>
        public List<StationDto> Stations { get; set; } = new List<StationDto>();

        /// <summary>
        /// 下行站点信息
        /// </summary>
        public List<StationDto> DownLinkStations { get; set; } = new List<StationDto>();

    }
}
