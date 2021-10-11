using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class TerminalDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public Guid EquipmentId { get; set; }
        public EquipmentDto Equipment { get; set; }

        /// <summary>
        /// 业务描述（即电缆芯说明）
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 和端子相连的线缆
        /// </summary>
        public List<TerminalLinkDto> TerminalLinkAs { get; set; }


        /// <summary>
        /// 和端子相连的线缆
        /// </summary>
        public List<TerminalLinkDto> TerminalLinkBs { get; set; }


        /// <summary>
        /// 和端子相连的线缆
        /// </summary>
        public List<TerminalLinkDto> TerminalLinks { get; set; }


        /// <summary>
        /// 端子业务路径
        /// </summary>
        public List<TerminalBusinessPathDto> TerminalBusinessPath { get; set; }
    }
}
