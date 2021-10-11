using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Exam.Dtos
{
    public class QuestionRltCategoryCreateDto:EntityDto
    {
        public Guid Id { get; set; }
    }
}
