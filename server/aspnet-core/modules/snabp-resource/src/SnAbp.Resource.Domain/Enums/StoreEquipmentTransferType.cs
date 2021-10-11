using System.ComponentModel;

namespace SnAbp.Resource.Enums
{
    /// <summary>
    /// 出入库类型
    /// </summary>
    public enum StoreEquipmentTransferType
    {
        [Description("入库")]
        Import = 1,

        [Description("出库")] 
        Export = 2,
    }
}
