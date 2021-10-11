using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Oa.Enums
{
    public enum UpdateType
    {
        [Description("解密Or加密")]
        DecodeOrEncryption = 1,

        [Description("有效")]
        effective = 2,

        [Description("无效")]
        Noneffective = 3,
    }
}
