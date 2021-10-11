using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Safe
{
    public enum SafeRecordState
    {
        [Description("检查中")]
        Checking = 1,
        [Description("不通过")]
        NotPass = 2,
        [Description("通过")]
        Passed = 3
    }
}
