using SnAbp.Exam.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Exam.IServices
{
    public interface IExamCategoryAppService : IApplicationService
    {
        Task<ExamCategoryDto> Get(Guid id);

        Task<PagedResultDto<ExamCategoryDto>> GetList(ExamCategorySearchDto dto);



        Task<ExamCategoryDto> Create(ExamCategoryCreateDto dto);

        Task<ExamCategoryDto> Update(ExamCategoryUpdateDto dto);

        Task<bool> Delete(Guid id);

        /// <summary>
        /// 获取分类管理树状列表
        /// </summary>
        Task<List<ExamCategoryDto>> GetTreeList(Guid? parentId);

        Task<List<ExamCategoryDto>> GetByParentId(Guid? parentId);

        Task<List<ExamCategoryDto>> GetParentsByIds(List<Guid> ids);
    }
}
