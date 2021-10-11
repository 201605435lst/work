using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
   public class ConstructInterfaceInfoReformDto : EntityDto<Guid>
    {
        /// <summary>
        /// 接口清单id
        /// </summary>
        public virtual Guid? ConstructInterfaceId { get; set; }
        /// <summary>
        /// 整改人
        /// </summary>
        public virtual Guid? ReformerId { get; set; }
        /// <summary>
        /// 整改时间
        /// </summary>
        public virtual DateTime? ReformDate { get; set; }

        /// <summary>
        /// 整改说明
        /// </summary>
        public virtual string ReformExplain { get; set; }

        public virtual List<ConstructInterfaceInfoRltMarkFileSimpleDto> MarkFiles { get; set; } = new List<ConstructInterfaceInfoRltMarkFileSimpleDto>();
    }
}
