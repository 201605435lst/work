using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 连接件表（安装位置，信号提示灯，开关等）
    /// </summary>
    public class StandardEquipmentTerminalSearchInputDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 标准设备
        /// </summary>
        public Guid ModelId { get; set; }

        /// <summary>
        /// 连接件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否获取所有
        /// </summary>
        public bool IsAll { get; set; }
    }
}