using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProjectItemRltProcessTemplateDto : EntityDto<Guid>
    {
        public Guid ProjectItemId { get; set; }
        public Guid ProcessTemplateId { get; set; }

        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public Guid SpecialtyId { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string SpecialtyName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 类型0（单项工程）1（工程工项）
        /// </summary>
        public int Type { get; set; }
    }
}
