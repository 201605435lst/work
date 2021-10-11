using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
  public  class ProjectItemRltIndividualProjectDto : EntityDto<Guid>
    {

        public  Guid ProjectItemId { get; set; }
        public  Guid IndividualProjectId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
