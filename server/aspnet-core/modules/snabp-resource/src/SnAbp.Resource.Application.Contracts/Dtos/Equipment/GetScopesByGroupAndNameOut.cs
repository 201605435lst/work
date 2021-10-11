using System;

namespace SnAbp.Resource.Dtos
{
    public class GetScopesByGroupAndNameOut
    {
        /// <summary>
        /// 设备 Id
        /// </summary>
        public Guid EquipmentId { get; set; }


        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }


        /// <summary>
        /// 设备分组名称
        /// </summary>
        public string EquipmentGroupName { get; set; }


        /// <summary>
        /// 范围字符串
        /// </summary>
        public string ScopeCode { get; set; }
    }
}