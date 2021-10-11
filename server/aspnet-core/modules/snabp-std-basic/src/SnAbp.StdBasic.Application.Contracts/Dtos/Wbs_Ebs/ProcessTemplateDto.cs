using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SnAbp.StdBasic.Enums;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProcessTemplateDto : EntityDto<Guid>, IGuidKeyTree<ProcessTemplateDto>
    {

        public  ProcessTemplateDto Parent { get; set; }
        public  List<ProcessTemplateDto> Children { get; set; } = new List<ProcessTemplateDto>();
        public  Guid? ParentId { get; set; }
        /// <summary>
        /// 前置任务id
        /// </summary>
        public  Guid? PrepositionId { get; set; }

        /// <summary>
        /// 前置任务编码
        /// </summary>
        [MaxLength(50)]
        [Description("前置任务编码")]
        public virtual string PrepositionCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        [MaxLength(50)]
        [Description("工序编码")]
        public  string Code { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        [MaxLength(200)]
        [Description("工序名称")]
        public  string Name { get; set; }
        /// <summary>
        /// 工作项单位
        /// </summary>
        [MaxLength(100)]
        [Description("工作项单位")]
        public  string Unit { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        [MaxLength(1000)]
        [Description("工作内容")]
        public string Content { get; set; }
        /// <summary>
        /// 工序类别
        /// </summary>
        [MaxLength(50)]
        [Description("工序类别")]
        public ProcessTypeEnum Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public  string Remark { get; set; }
        /// <summary>
        /// 工期
        /// </summary>
        public decimal Duration { get; set; }
        /// <summary>
        /// 工期单位
        /// </summary>
        public ServiceLifeUnit DurationUnit { get; set; }
    }
}
