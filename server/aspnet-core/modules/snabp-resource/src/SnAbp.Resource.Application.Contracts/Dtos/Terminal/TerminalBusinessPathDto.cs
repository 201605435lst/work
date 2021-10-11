using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    /// <summary>
    /// 端子业务表
    /// </summary>
    public class TerminalBusinessPathDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }


        /// <summary>
        /// 配线径路
        /// </summary>
        public List<TerminalBusinessPathNodeDto> Nodes { get; set; }
    }
}
