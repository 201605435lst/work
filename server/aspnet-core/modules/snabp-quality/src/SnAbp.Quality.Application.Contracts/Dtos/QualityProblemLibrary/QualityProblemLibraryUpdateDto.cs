using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemLibraryUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 问题名称
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        public QualityProblemType Type { get; set; }
        /// <summary>
        /// 问题等级
        /// </summary>
        public QualityProblemLevel Level { get; set; }
        /// <summary>
        /// 所属专业，数字字典
        /// </summary>
        public virtual Guid ProfessionId { get; set; }
        /// <summary>
        /// 问题内容
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        public virtual string Measures { get; set; }

        /// <summary>
        /// 适用范围，为多选的数字字典
        /// </summary>
        public virtual List<QualityProblemLibraryRltScopCreateDto> Scops { get; set; }
    }
}
