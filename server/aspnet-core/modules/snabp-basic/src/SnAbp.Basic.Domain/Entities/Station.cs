using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Basic.Entities
{
    /// <summary>
    /// 站点表
    /// </summary>
    public class Station : FullAuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        [StringLength(50)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 类型  0车站  1区间
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 区间起始站
        /// </summary>
        [ForeignKey("SectionStartStation")]
        public Guid? SectionStartStationId { get; set; }
        public Station SectionStartStation { get; set; }

        /// <summary>
        /// 区间终止站
        /// </summary>
        [ForeignKey("SectionEndStation")]
        public Guid? SectionEndStationId { get; set; }
        public Station SectionEndStation { get; set; }

        /// <summary>
        /// 公里标
        /// </summary>
        //public int KMMark { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500)]
        [Description("备注")]
        public string Remark { get; set; }

        protected Station() { }
        public Station(Guid id)
        {
            Id = id;
        }
    }
}
