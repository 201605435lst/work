using SnAbp.File.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class EmergPlanRecordRltFileSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual FileSimpleDto File { get; set; }

    }
}
