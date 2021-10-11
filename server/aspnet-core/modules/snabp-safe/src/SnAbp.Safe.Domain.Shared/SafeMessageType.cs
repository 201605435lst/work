using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Safe
{
    public enum SafeMessageType
    {
        [Description("发送给整改人")]
        ImproveMessage = 1,
        [Description("发送给验证人")]
        VerifyMessage = 2,
        [Description("未通过的整改消息")]
        ImproveMessageNotPass = 3,
        [Description("通过的整改消息")]
        ImproveMessagePassed = 4
    }
}