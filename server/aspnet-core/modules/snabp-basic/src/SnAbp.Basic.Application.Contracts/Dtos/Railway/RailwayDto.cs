using SnAbp.Basic.Dtos;
using SnAbp.Basic.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class RailwayDto : EntityDto<Guid>
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
