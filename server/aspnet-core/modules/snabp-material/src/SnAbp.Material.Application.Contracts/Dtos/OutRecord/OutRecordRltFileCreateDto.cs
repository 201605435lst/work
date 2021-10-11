using System;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 出库关联资料
    /// </summary>
    public class OutRecordRltFileCreateDto
    {
        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
    }
}
