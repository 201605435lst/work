using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos.Model
{
    public class ModelFileCreateDto
    {
        /// <summary>
        /// 模型Id
        /// </summary>
        public Guid? ModelId { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public Guid? ThumbId { get; set; }

        /// <summary>
        /// 族文件
        /// </summary>
        public Guid? FamilyFileId { get; set; }

        /// <summary>
        /// 模型精细等级
        /// </summary>
        public ModelDetailLevel DetailLevel { get; set; }

    }
}
