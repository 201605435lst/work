using System;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联文件
    /// </summary>
    public class DispatchRltFileCreateDto
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public virtual Guid FileId { get; set; }
    }
}
