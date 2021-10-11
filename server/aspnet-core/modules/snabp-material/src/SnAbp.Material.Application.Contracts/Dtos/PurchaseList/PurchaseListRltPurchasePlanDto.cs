﻿using SnAbp.Identity;
using SnAbp.Material.Entities;
using SnAbp.Material.Enums;
using SnAbp.Project.Dtos;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 采购计划与采购单关联表
    /// </summary>
    public class PurchaseListRltPurchasePlanDto : EntityDto<Guid>
    {
        /// <summary>
        /// 采购计划
        /// </summary>
        public Guid PurchasePlanId { get; set; }
        public PurchasePlan PurchasePlan { get; set; }
        /// <summary>
        /// 采购清单
        /// </summary>
        public Guid PurchaseListId { get; set; }
        public PurchaseList PurchaseList { get; set; }
    }
}
