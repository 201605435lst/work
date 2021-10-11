using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 设备属性
    /// </summary>
    public class CableExtend : FullAuditedEntity<Guid>
    {
        protected CableExtend() { }
        public CableExtend(Guid id) { Id = id; }


        /// <summary>
        /// 芯数
        /// </summary>
        public int? Number { get; set; }


        /// <summary>
        /// 备用芯数
        /// </summary>
        public int? SpareNumber { get; set; }


        /// <summary>
        /// 路产芯数
        /// </summary>
        public int? RailwayNumber { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }
        /// <summary>
        /// 皮长公里
        /// </summary>
        public float? Length { get; set; }


        /// <summary>
        /// 铺设类型
        /// </summary>
        public CableLayType? LayType { get; set; }
    }
}
