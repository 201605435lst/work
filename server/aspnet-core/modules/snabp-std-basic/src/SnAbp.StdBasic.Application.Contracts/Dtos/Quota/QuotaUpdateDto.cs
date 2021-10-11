using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Dtos
{
    public class QuotaUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 定额分类
        /// </summary>
        public  Guid QuotaCategoryId { get; set; }
        /// <summary>
        /// 定额名称
        /// </summary>
        public  string Name { get; set; }
        /// <summary>
        /// 定额编码
        /// </summary>
        public  string Code { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public  string Unit { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public float Weight { get; set; }
        /// <summary>
        /// 人工费
        /// </summary>
        public float LaborCost { get; set; }
        /// <summary>
        /// 材料费
        /// </summary>
        public float MaterialCost { get; set; }
        /// <summary>
        /// 机械使用费
        /// </summary>
        public  float MachineCost { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public  string Remark { get; set; }
    }
}
