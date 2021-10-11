using SnAbp.File.Dtos;
using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Model
{
    public class ModelFileDto:EntityDto<Guid>
    {
        /// <summary>
        /// 模型Id
        /// </summary>
        public Guid? ModelId { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public Guid? ThumbId { get; set; }
        public virtual FileSimpleDto Thumb { get; set; }

        /// <summary>
        /// 族文件
        /// </summary>
        public Guid? FamilyFileId { get; set; }
        public virtual FileSimpleDto FamilyFile { get; set; }

        /// <summary>
        /// 模型精细等级
        /// </summary>
        public ModelDetailLevel DetailLevel { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Ext { get; set; }

    }




}