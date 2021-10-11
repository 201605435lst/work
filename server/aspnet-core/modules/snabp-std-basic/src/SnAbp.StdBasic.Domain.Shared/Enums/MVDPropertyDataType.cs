using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.StdBasic.Enums
{
    public enum MVDPropertyDataType
    {
        [Description("尺寸")]
        Length = 1,
        [Description("数值")]
        Digit = 2,
        [Description("文本")]
        String = 3
    }
}
