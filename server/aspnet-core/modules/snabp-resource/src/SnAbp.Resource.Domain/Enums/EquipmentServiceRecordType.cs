using System.ComponentModel;

namespace SnAbp.Resource.Enums
{
    /// <summary>
    /// 设备使用记录
    /// </summary>
    public enum EquipmentServiceRecordType
    {

        [Description("按照")] // 上道
        Install = 1,

        [Description("拆除")] // 下道
        UnInstall = 2,
    }
}
