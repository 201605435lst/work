using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class CableCoreDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电缆
        /// </summary>
        public Guid CableId { get; set; }
        public EquipmentDto Cable { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public CableCoreType Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Rremark { get; set; }
        public Guid TerminalLinkId { get; set; }
        public string TerminalAName { get; set; }
        public string EquipmentAName { get; set; }
        public string EquipmentAGroupName { get; set; }
        public string EquipmentBName { get; set; }
        public string EquipmentBGroupName { get; set; }
        public string TerminalBName { get; set; }
        public string BusinessFunction { get; set; }
        public List<TerminalBusinessPathDto> TerminalBusinessPaths { get; set; }
    }
}
