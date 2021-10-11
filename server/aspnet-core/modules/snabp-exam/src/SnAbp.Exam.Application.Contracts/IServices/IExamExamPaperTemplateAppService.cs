using SnAbp.Exam.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Exam.IServices
{
    public interface IExamExamPaperTemplateAppService : IApplicationService
    {
        Task<ExamPaperTemplateDto> Create(ExamPaperTemplateCreateDto input);
        Task<ExamPaperTemplateDto> Update(ExamPaperTemplateUpdateDto input);
        Task<PagedResultDto<ExamPaperTemplateDto>> GetList(ExamPaperTemplateSearchDto input);

        Task<ExamPaperTemplateDto> Get(Guid id);
        Task<bool> Delete(List<Guid> ids);
    }
}
