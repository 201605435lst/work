using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Schedule.Enums
{
    public enum DiaryRltBuilderType
    {
        [Description("负责人")]
        Director = 1,
        [Description("施工人")]
        Builder = 2,
    }
}
