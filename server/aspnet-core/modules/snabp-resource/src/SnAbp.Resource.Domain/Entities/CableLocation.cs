using NPOI.OpenXmlFormats.Wordprocessing;
using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 电缆位置信息（埋深）
    /// </summary>
    public class CableLocation : FullAuditedEntity<Guid>
    {

        protected CableLocation() { }
        public CableLocation(Guid id) { Id = id; }


        /// <summary>
        /// 电缆 Id
        /// </summary>
        public Guid CableId { get; set; }
        public Equipment Cable { get; set; }


        [MaxLength(100)]
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 参考方向
        /// </summary>
        public CableLocationDirection Direction { get; set; }


        /// <summary>
        /// 参考值
        /// </summary>
        public float Value { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }
        /// <summary>
        /// 位置数据（Json String）
        /// </summary>
        public string Positions { get; set; }
    }
}
