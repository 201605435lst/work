using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SnAbp.Resource.Dtos
{
    public class GetScopesByGroupAndNameInput
    {
        /// <summary>
        /// 设备分组+名称 [ Group@Name ]
        /// </summary>
        public List<string> EquipmentGroupAndNames { get; set; }
    }
}