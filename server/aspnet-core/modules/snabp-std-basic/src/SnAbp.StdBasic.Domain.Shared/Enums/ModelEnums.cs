using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.StdBasic.Enums
{
    /// <summary>
    /// 模型精细等级
    /// </summary>
    public enum ModelDetailLevel
    {
        [Description("1级")]
        GradeI = 1,
        GradeII = 2,
        GradeIII = 3,
        GradeIV = 4,
    }
}
