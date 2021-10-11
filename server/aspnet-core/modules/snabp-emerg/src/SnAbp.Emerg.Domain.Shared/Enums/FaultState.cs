using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Emerg.Enums
{
    public enum FaultState
    {
        [Description("待提交")]
        UnSubmitted = 1,

        [Description("已提交")]
        Submitted = 2,

        [Description("待处理")]
        Pending = 3,

        [Description("未销记")]
        UnChecked = 4,

        [Description("已销记")]
        CheckedOut = 5,
    }
}
