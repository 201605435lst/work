using SnAbp.File.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class ArticleSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 概要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 缩略图文件id
        /// </summary>
        public virtual Guid ThumbId { get; set; }

        /// <summary>
        /// 缩略图文件
        /// </summary>
        public virtual FileSimpleDto Thumb { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 栏目
        /// </summary>
        public List<string> Categories { get; set; }
    }
}
