using SnAbp.Identity;
using SnAbp.Technology.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
   public class ConstructInterfaceInfoDto : EntityDto<Guid>
    {
        /// <summary>
        /// 接口清单id
        /// </summary>
        public virtual Guid? ConstructInterfaceId { get; set; }
        public virtual ConstructInterfaceDto ConstructInterface { get; set; }
        /// <summary>
        /// 检查人员
        /// </summary>
        public virtual Guid MarkerId { get; set; }
        public virtual IdentityUser Marker { get; set; }
        /// <summary>
        /// 土建单位
        /// </summary>
        public virtual Guid BuilderId { get; set; }
        public virtual DataDictionary Builder { get; set; }
        /// <summary>
        /// 接口检查情况
        /// </summary>
        public virtual MarkType MarkType { get; set; }

        /// <summary>
        /// 检查原因
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// 标记时间
        /// </summary>
        public virtual DateTime? MarkDate { get; set; }

        /// <summary>
        /// 整改人
        /// </summary>
        public virtual Guid? ReformerId { get; set; }
        public virtual IdentityUser Reformer { get; set; }
        /// <summary>
        /// 整改时间
        /// </summary>
        public virtual DateTime? ReformDate { get; set; }

        /// <summary>
        /// 整改说明
        /// </summary>
        public virtual string ReformExplain { get; set; }

        public virtual List<ConstructInterfaceInfoRltMarkFileDto> MarkFiles { get; set; } = new List<ConstructInterfaceInfoRltMarkFileDto>();
    }
}
