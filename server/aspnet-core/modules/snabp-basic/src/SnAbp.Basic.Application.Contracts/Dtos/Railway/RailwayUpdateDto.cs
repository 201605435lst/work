using SnAbp.Basic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SnAbp.Basic.Dtos
{
    public class RailwayUpdateDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型  枚举  0单线 1复线
        /// </summary>
        public RailwayType Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 线路组织机构关联
        /// </summary>
        public List<RailwayOrgCreateDto> RailwayOrgs { get; set; }
    }
}
