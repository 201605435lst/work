using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Emerg.Enums
{
    public enum FaultSource
    {
        // 历史数据维护，无预案
        [Description("历史记录")]
        History = 1,

        // 走系统登记、预案，消记流程，有预案
        [Description("系统登记")]
        System = 2,
    }
}
