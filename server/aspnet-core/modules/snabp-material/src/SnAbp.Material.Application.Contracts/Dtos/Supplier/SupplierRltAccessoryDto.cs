
using System;
using SnAbp.File.Dtos;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class SupplierRltAccessoryDto : EntityDto<Guid>
    {
        /// <summary>
        /// 供应商id
        /// </summary>
        public virtual Guid SupplierId { get; set; }
        public virtual SupplierSimpleDto Supplier { get; set; }

        /// <summary>
        /// 文件id
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual FileSimpleDto File { get; set; }
    }
}
