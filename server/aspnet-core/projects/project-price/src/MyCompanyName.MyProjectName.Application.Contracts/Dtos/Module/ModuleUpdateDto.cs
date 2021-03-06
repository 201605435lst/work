using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyCompanyName.MyProjectName.Dtos
{
    /// <summary>
    /// 模块
    /// </summary>
    public class ModuleUpdateDto : EntityDto<Guid>
    {
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// 工作天数（人/天)
        /// </summary>
        public float WorkDays { get; set; }

        /// <summary>
        /// 完成进度
        /// </summary>
        public float Progress { get; set; }
    }
}
