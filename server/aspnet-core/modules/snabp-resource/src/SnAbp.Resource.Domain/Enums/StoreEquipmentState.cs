using System.ComponentModel;

namespace SnAbp.Resource.Enums
{
    /// <summary>
    /// 设备运行状态
    /// </summary>
    public enum StoreEquipmentState
    {

        [Description("未激活")] // 设备首次编码，以后未激活
        UnActived = 1,

        [Description("已安装")] // 已上道/已安装
        OnService = 2,

        [Description("待检测")]
        WaitForTest = 3,

        [Description("备用")]  // 检测合格
        Spare = 4,

        [Description("报废")]
        Scrap = 5
    }
}
