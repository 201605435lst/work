using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Safe
{
    /// <summary>
    /// $$
    /// </summary>
    public enum SafeFilterType
    {
        [Description("全部")]
        All = 1,
        [Description("我检查的")]
        MyChecked = 2,
        [Description("待我整改")]
        MyWaitingImprove = 3,
        [Description("待我验证")]
        MyWaitingVerify = 4,
        [Description("抄送我的")]
        CopyMine = 5
    }
}