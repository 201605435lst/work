using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class StationSimpleDto : EntityDto<Guid>
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
                    default:
                        break;
                }
                return res;
            }
        }

        /// <summary>
        /// 所属线路字符串集合
        /// </summary>
        public string BelongRailways { get; set; }

        /// <summary>
        /// 维护班组字符串集合
        /// </summary>
        public string RepairTeams { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
