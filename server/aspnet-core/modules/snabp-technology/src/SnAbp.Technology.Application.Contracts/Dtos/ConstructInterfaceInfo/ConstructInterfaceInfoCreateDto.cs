using SnAbp.Technology.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
   public class ConstructInterfaceInfoCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 接口清单id
        /// </summary>
        public virtual Guid? ConstructInterfaceId { get; set; }
        /// <summary>
        /// 检查人员
        /// </summary>
        public virtual Guid MarkerId { get; set; }
        /// <summary>
        /// 土建单位
        /// </summary>
        public virtual Guid BuilderId { get; set; }
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


        public virtual List<ConstructInterfaceInfoRltMarkFileSimpleDto> MarkFiles { get; set; } = new List<ConstructInterfaceInfoRltMarkFileSimpleDto>();
    }
}
