using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Project.enums
{
    public enum Security
    {
        [Description("绝密")]
        Strice = 1,
        [Description("机密")]
        Secret = 2,
        [Description("普通")]
        Common = 3,

    }
}
