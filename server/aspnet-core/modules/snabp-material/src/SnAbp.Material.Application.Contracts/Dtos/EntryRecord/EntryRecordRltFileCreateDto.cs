using SnAbp.File.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 入库关联资料
    /// </summary>
    public class EntryRecordRltFileCreateDto
    {
        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
    }
}
