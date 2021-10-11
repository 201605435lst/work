using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowTemplateSearchInputDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 工作流名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool? Published { get; set; }


        /// <summary>
        /// 待我发起
        /// </summary>
        public bool ForCurrentUser { get; set; } = false;

        /// <summary>
        /// 分组id
        /// </summary>
        public Guid? GroupId { get; set; }
        /// <summary>
        /// 是否选择模式
        /// </summary>
        public bool Select { get; set; }
        /// <summary>
        /// 是否选择模式
        /// </summary>
        public bool IsAll { get; set; }
    }
}