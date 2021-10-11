using SnAbp.File.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联文件
    /// </summary>
    public class DispatchRltFileDto : EntityDto<Guid>
    {
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual DispatchDto Dispatch { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual FileSimpleDto File { get; set; }
    }
}
