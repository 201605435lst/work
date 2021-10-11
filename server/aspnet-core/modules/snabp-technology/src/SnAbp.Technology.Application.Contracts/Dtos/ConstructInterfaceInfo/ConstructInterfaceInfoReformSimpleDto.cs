using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
   public class ConstructInterfaceInfoReformSimpleDto:EntityDto<Guid>
    {
        /// <summary>
        /// 接口清单id
        /// </summary>
        public virtual Guid? ConstructInterfaceId { get; set; }
    }
}
