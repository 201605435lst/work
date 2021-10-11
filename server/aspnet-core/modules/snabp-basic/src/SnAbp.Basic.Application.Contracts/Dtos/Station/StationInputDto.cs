using SnAbp.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SnAbp.Basic.Dtos
{
    public class StationInputDto
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string Name { get; set; }        

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 维护班组
        /// </summary>
        public List<Guid> RepairTeam { get; set; } = new List<Guid>();
    }
}
