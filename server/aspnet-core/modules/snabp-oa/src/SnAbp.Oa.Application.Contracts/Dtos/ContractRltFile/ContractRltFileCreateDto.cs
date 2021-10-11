using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos
{
    public class ContractRltFileCreateDto:EntityDto<Guid>
    {
        public Guid ContractId { get; set; }
      

        public Guid FileId { get; set; }
       
    }
}
