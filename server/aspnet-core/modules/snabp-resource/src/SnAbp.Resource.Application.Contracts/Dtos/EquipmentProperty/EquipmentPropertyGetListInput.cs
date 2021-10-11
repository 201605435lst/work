using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentPropertyGetListInput
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        public string EquipmentGroupName { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }
    }
}
