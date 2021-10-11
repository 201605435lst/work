using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Schedule.Enums
{
    public enum MaterialsType
    {
        [Description("辅助材料")]
        AutoCompute = 1,
        [Description("使用器具")]
        Appliance = 2,
        [Description("使用机械")]
        Mechanical = 3,
        [Description("安全防护用品")]
        SafetyArticle = 4,
    }
    public enum StatusType
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
}
