using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Dtos
{
    public class QuotaItemEditDto : Entity<Guid>
    {
        /// <summary>
        /// 基价Id
        /// </summary>
        public Guid BasePriceId { get; set; }

        /// <summary>
        /// 基础单价
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// 标准编号
        /// </summary>
        public string StandardCodeName { get; set; }

        /// <summary>
        /// 标准编号Id
        /// </summary>
        public Guid StandardCodeId { get; set; }

        /// <summary>
        /// 行政区划Id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 行政区划Name
        /// </summary>
        public string AreaName { get; set; }

    }
}
