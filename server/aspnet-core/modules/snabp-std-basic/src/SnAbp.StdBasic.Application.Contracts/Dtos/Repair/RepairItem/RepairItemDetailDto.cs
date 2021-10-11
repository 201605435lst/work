using SnAbp.StdBasic.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;
using SnAbp.StdBasic.Entities;

namespace SnAbp.StdBasic.Dtos
{
    public class RepairItemDetailDto : EntityDto<Guid>
    {

        /// <summary>
        /// 维修类型
        /// </summary>
        public RepairType Type { get; set; }

        /// <summary>
        /// 维修分组
        /// </summary>
        public Guid GroupId { get; set; }
        public RepairGroupDto Group { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 周期：每年/月次数
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// 周期单位：每年/月
        /// </summary>
        public RepairPeriodUnit PeriodUnit { get; set; }

        /// <summary>
        /// 是否为月维修项
        /// </summary>
        public bool IsMonth { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 测试项
        /// </summary>
        public List<RepairTestItemDto> StandTestItems { get; set; }


        /// <summary>
        /// 构件分类
        /// </summary>
        public List<RepairItemRltComponentCategoryDto> RepairItemRltComponentCategories { get; set; }

        /// <summary>
        /// 维修标签
        /// </summary>
        public Guid TagId { get; set; }

        /// <summary>
        /// 执行单位
        /// </summary>
        public List<RepairItemRltOrganizationType> RepairItemRltOrganizationTypes { get; set; }

    }
}
