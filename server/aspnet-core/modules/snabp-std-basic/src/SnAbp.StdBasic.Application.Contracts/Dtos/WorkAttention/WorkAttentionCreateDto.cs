using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Dtos
{
    public class WorkAttentionCreateDto
    {


        /// <summary>
        /// 类别Id
        /// </summary>
        public Guid? TypeId { get; set; }
       

        /// <summary>
        /// 数据是否是“类别”类型
        /// </summary>
        public bool IsType { get; set; }

        /// <summary>
        /// 注意事项 或 类别名称
        /// </summary>
        public string Content { get; set; }

        public string RepairTagKey { get; set; }


    }
}
