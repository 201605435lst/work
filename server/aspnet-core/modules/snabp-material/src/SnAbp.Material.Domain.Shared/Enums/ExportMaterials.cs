using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Material.Enums
{
    public enum ExportMaterials
    {
        材料名称,
        材料类别,
        所属专业,
        所属设备,
        材料规格,
        材料型号,
        材料价格,
        单位,
        备注,
    }
    public enum ExportPurchases
    {
        序号,
        专业,
        名称,
        类别,
        型号,
        规格,
        单位,
        数量,
        单价,
        合计,
    }
    public enum ExportUsePlan
    {
        序号,
        名称,
        计划时间,
        创建时间,
        创建人,
        审核状态,
    }
    public enum ExportMaterialsForInquire
    {
        材料名称,
        材料类别,
        所属专业,
        料库地点,
        材料规格,
        材料型号,
        单位,
        库存,
        计划用料,
        量差,
    }
}
