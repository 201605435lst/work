using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
   public class ConstructionTeamDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 施工点
        /// </summary>
        public virtual Guid ConstructionSectionId { get; set; }
        public virtual ConstructionSectionDto ConstructionSection { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string PeopleName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
    }
}
