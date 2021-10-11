using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Dtos
{
    public class QuotaItemDto
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
        /// 电算代号
        /// </summary>
        public string ComputerCode { get; set; }

        /// <summary>
        /// 电算代号名称及规格
        /// </summary>
        public string ComputerCodeName { get; set; }

        /// <summary>
        /// 基价列表
        /// </summary>
       public List<QuotaItemEditDto> QuotaItemEditList { get; set; }

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
