using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Dtos
{
  public  class QuotaItemUpdateDto : EntityDto<Guid>
    {
       

        public  Guid QuotaId { get; set; }
        
        //public  Guid BasePriceId { get; set; }

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
        public  float Number { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public  string Remark { get; set; }
    }
}
