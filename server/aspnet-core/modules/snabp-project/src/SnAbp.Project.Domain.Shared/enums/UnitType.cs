using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Project.enums
{
    public enum UnitType
    {
        [Description("供应商")]
        Supplier = 1,
        [Description("业主")]
        Owner = 2,
    }
}
