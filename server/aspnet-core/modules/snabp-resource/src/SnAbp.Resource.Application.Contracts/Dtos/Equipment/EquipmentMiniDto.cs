using SnAbp.StdBasic.Dtos;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentMiniDto : EntityDto<Guid>
    {
        /// <summary>
        /// 编号
        /// </summary>
        [MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// CSRG编号
        /// </summary>
        [MaxLength(50)]
        public string CSRGCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 构件编码
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }
        public ComponentCategoryDto ComponentCategory { get; set; }
    }
}
