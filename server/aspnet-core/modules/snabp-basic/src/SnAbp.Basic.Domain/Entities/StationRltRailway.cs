using SnAbp.Basic.Enums;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Basic.Entities
{
    /// <summary>
    /// 线路站点对应表
    /// </summary>
    public class StationRltRailway : Entity<Guid>
    {
         public Guid? ProjectTagId { get; set; }

        /// <summary>
        /// 线路Guid
        /// </summary>
        [ForeignKey("Railway")]
        public Guid RailwayId { get; set; }
        public virtual Railway Railway { get; set; }

        /// <summary>
        /// 站点Guid
        /// </summary>
        [ForeignKey("Station")]
        public Guid StationId { get; set; }
        public Station Station { get; set; }

        /// <summary>
        /// 经过顺序
        /// </summary>
        public int PassOrder { get; set; }

        /// <summary>
        /// 公里标
        /// </summary>
        public int KMMark { get; set; }

        /// <summary>
        /// 关联的线路类型
        /// </summary>
        public RelateRailwayType RailwayType { get; set; }

        protected StationRltRailway() { }
        public StationRltRailway(Guid id)
        {
            Id = id;
        }
    }
}
