using SnAbp.Material.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class SupplierSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public SupplierType Type { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string Principal { get; set; }

        /// <summary>
        /// 法人代表
        /// </summary>
        public string LegalPerson { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        public string BusinessScope { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
    }
}
