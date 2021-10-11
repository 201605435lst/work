using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Material.Enums
{
    public enum PurchaseState
    {
        [Description("待提交")]
        ToSubmit = 1,
        [Description("审核中")]
        OnReview = 2,
        [Description("审核通过")]
        Pass = 3,
        [Description("审核未通过")]
        UnPass = 4,
    }

    public enum PurchaseListType
    {
        [Description("按月采购")]
        MonthPurchasing = 1,
        [Description("集中采购")]
        CentralPurchasing = 2,
        [Description("零星采购")]
        MinorPurchasing = 3,
    }
}
