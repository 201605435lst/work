﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Dtos
{
    public class BasePriceUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 电算代号Id
        /// </summary>
        public Guid ComputerCodeId { get; set; }

        /// <summary>
        /// 基础单价
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// 标准编号
        /// </summary>
        public Guid StandardCodeId { get; set; }
        /// <summary>
        /// 行政区划Id
        /// </summary>
        public int AreaId { get; set; }
    }
}
