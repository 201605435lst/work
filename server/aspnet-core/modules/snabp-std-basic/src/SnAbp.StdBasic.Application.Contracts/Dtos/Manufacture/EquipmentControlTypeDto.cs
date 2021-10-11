using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class EquipmentControlTypeDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 厂家
        /// </summary>
        public Guid? ManufactureId { get; set; }
        public virtual ManufacturerDto Manufacturer { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string TypeGroup { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
