using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.StdBasic.Enums;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProcessTemplateUpdateDto : EntityDto<Guid>
    {

        public  Guid? ParentId { get; set; }
        /// <summary>
        /// 前置任务id
        /// </summary>
        public  Guid? PrepositionId { get; set; }
   
        /// <summary>
        /// 工序编码
        /// </summary>
        public  string Code { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public  string Name { get; set; }
        /// <summary>
        /// 工作项单位
        /// </summary>
        public  string Unit { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public  string Content { get; set; }
        /// <summary>
        /// 工序类别
        /// </summary>
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
