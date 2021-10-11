using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    /// <summary>
    /// 端子业务径路表
    /// </summary>
    public class TerminalBusinessPathNodeDto : EntityDto<Guid>
    {
        /// <summary>
        /// 端子业务表
        /// </summary>
        public TerminalBusinessPathDto TerminalBusinessPath { get; set; }
        public Guid TerminalBusinessPathId { get; set; }


        /// <summary>
        /// 端子 Id
        /// </summary>
        public TerminalDto Terminal { get; set; }
        public Guid TerminalId { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// 电缆芯 Id
        /// </summary>
        public Guid? CableCoreId { get; set; }
        public CableCoreDto CableCore { get; set; }
    }
}
