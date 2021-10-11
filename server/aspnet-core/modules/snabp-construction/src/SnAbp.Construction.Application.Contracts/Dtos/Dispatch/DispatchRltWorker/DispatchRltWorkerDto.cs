using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联人员
    /// </summary>
    public class DispatchRltWorkerDto : EntityDto<Guid>
    {
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual DispatchDto Dispatch { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public virtual Guid WorkerId { get; set; }
        public virtual IdentityUserDto Worker { get; set; }
    }
}
