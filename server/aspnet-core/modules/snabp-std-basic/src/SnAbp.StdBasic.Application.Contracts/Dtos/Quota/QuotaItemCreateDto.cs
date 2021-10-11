using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos
{
    public class QuotaItemCreateDto
    {
        /// <summary>
        /// 定额Id
        /// </summary>
        public Guid QuotaId { get; set; }

        /// <summary>
        /// 电算代号Id
        /// </summary>
        public Guid ComputerCodeId { get; set; }

        /// <summary>
        /// 基价Id
        /// </summary>
        public List<Guid> BasePriceIdList { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public float Number { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
