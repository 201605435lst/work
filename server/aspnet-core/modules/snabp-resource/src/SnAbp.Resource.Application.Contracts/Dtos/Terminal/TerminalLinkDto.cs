using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class TerminalLinkDto : EntityDto<Guid>
    {
        /// <summary>
        /// 端子ID
        /// </summary>
        public Guid TerminalAId { get; set; }
        public TerminalDto TerminalA { get; set; }


        /// <summary>
        /// 对方端子ID
        /// </summary>
        public Guid TerminalBId { get; set; }
        public TerminalDto TerminalB { get; set; }


        /// <summary>
        /// 对方端子ID
        /// </summary>
        public Guid TargetTerminalId { get; set; }
        public TerminalDto TargetTerminal { get; set; }


        /// <summary>
        /// 线缆芯
        /// </summary>
        public Guid? CableCoreId { get; set; }
        public CableCoreDto CableCore { get; set; }


        public List<TerminalBusinessPathDto> TerminalBusinessPaths { get; set; }
    }
}
