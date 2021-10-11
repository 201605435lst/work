using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Dtos
{
    public class ComputerCodeDto : Entity<Guid>
    {
        /// <summary>
        /// 电算代号
        /// </summary>
        [MaxLength(50)]
        [Description("电算代号")]
        public string Code { get; set; }
        /// <summary>
        /// 名称及规格
        /// </summary>
        [MaxLength(200)]
        [Description("名称及规格")]
        public string Name { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        [MaxLength(50)]
        [Description("单位")]
        public string Unit { get; set; }
        /// <summary>
        /// 电算代号类型
        /// </summary>
        [Description("电算代号类型")]
        public ComputerCodeType Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 单位重量
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// 基价
        /// </summary>
        public List<BasePriceDto> BasePrices { get; set; }
    }
}
