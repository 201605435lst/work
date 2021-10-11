using SnAbp.File.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class SupplierRltAccessoryCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public virtual Guid FileId { get; set; }
    }
}
