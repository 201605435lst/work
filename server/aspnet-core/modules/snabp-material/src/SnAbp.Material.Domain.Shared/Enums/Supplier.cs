using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Material.Enums
{
    //供应商类型
    public enum SupplierType
    {
        [Description("供应商")]
        Supplier = 1,
        [Description("业主")]
        Proprietor = 2,
        [Description("施工队")]
        ConstructionTeam = 3
    }

    //供应商级别
    public enum SupplierLevel
    {
        [Description("一级供应商")]
        LevelI = 1,
        [Description("二级供应商")]
        LevelII = 2,
        [Description("三级供应商")]
        LevelIII = 3
    }

    //供应商性质
    public enum SupplierProperty
    {
        [Description("单位")]
        Unit = 1,
        [Description("个人")]
        Personal = 2
    }

    public enum ExportSuppliers
    {
        供应商名称,
        供应商类型,
        供应商级别,
        供应商性质,
        供应商编码,
        负责人姓名,
        负责人电话,
        法定代表人,
        纳税人识别号,
        经营范围,
        开户行名称,
        开户行账户,
        开户单位,
        注册资本,
        地址,
        备注
    }
}
