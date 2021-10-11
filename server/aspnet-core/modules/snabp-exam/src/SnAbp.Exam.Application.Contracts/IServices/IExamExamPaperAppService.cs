using SnAbp.Exam.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Exam.IServices
{
    public interface IExamExamPaperAppService:IApplicationService
    {
        Task<ExamPaperDto> Create(ExamPaperCreateDto input);
        Task<Boolean> Delete(List<Guid> id);
       // Task<Boolean> Delete(Guid id);
        Task<ExamPaperDto> Update(ExamPaperUpdateDto input);
        Task<PagedResultDto<ExamPaperDto>> GetList(ExamPaperSearchDto input);
        Task<ExamPaperDto> Get(Guid id);
        Task<PagedResultDto<ExamPaperRltQuestionConfigDto>> GetQuestionList(ExamPaperRltQuestionSearchDto input);
    }
}
