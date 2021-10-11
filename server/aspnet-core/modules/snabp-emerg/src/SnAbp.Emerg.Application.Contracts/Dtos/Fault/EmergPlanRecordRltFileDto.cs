using SnAbp.Emerg.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class EmergPlanRecordRltFileDto : EntityDto<Guid>
    {      
      /// <summary>
      /// 预案id
      /// </summary>
        public virtual Guid EmergPlanId { get; set; }
        public virtual EmergPlanRecordSimpleDto EmergPlanRecord { get; set; }

        /// <summary>
        /// 文件id
        /// </summary>
        public virtual Guid FileId { get; set; }
    }
}
