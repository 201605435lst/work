using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Exam.Dtos
{
    public class KnowledgePointRltCategorySimpleDto : EntityDto<Guid>
    {
        //分类Id
        public Guid CategoryId { get; set; }
        public ExamCategorySimpleDto Category { get; set; }
    }
}
