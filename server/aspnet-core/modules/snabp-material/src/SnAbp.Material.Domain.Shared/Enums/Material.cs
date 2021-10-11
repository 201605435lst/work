using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Material.Enums
{
    public enum Category
    {
        [Description("辅助材料")]
        Auxiliary = 1,
        [Description("器具")]
        Appliance = 2,
        [Description("机械")]
        Mechanical = 3,
        [Description("安全防护用品")]
        SafetyArticle = 4
    }
}
