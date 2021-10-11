using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class RepairItemCreateDto
    {
        [Required]
        /// <summary>
        /// 维修分组
        /// </summary>
        public Guid GroupId { get; set; }

        [Required]
        /// <summary>
        /// 维修类型
        /// </summary>
        public RepairType Type { get; set; }

        [Required]
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }

        [Required]
        /// <summary>
        /// 工作内容
        /// </summary>
        public string Content { get; set; }

        [Required]
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        [Required]
        /// <summary>
        /// 周期：每年/月次数
        /// </summary>
        public string Period { get; set; }

        [Required]
        /// <summary>
        /// 周期单位：每年/月
        /// </summary>
        public RepairPeriodUnit PeriodUnit { get; set; }

        [Required]
        /// <summary>
        /// 是否为月维修项
        /// </summary>
        public bool IsMonth { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 构件分类 ids
        /// </summary>
        public List<Guid> ComponentCategoryIds { get; set; }


        public string RepairTagKey { get; set; }

        /// <summary>
        /// 执行单位ids
        /// </summary>
        public List<Guid> OrganizationTypeIds { get; set; }
    }
}
