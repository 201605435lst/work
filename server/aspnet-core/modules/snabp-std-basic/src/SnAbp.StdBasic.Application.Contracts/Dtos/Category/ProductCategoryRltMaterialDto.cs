using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProductCategoryRltMaterialDto : EntityDto<Guid>
    {
        public Guid ProductCategoryId { get; set; }

        public Guid ComputerCodeId { get; set; }

        /// <summary>
        /// 电算代号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称及规格
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 单位重量
        /// </summary>
        public float Weight { get; set; }
    }
}
