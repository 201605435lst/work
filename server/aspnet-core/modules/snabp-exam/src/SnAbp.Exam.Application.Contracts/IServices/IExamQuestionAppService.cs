using SnAbp.Exam.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Exam.IServices
{
    public interface IExamQuestionAppService : IApplicationService
    {
        Task<QuestionDto> Get(Guid id);
        Task<PagedResultDto<QuestionSimpleDto>> GetList(QuestionSearchDto input);
        Task<QuestionDto> Create(QuestionCreateDto input);
        Task<QuestionDto> Update(QuestionUpdateDto input);
        Task<bool> delete(List<Guid> ids);
    }
}
