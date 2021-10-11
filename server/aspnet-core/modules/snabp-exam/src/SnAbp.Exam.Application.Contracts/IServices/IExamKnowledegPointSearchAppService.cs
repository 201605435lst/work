using SnAbp.Exam.Dtos;
using SnAbp.Exam.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Exam.IServices
{
    public interface  IExamKnowledegPointSearchAppService : IApplicationService
    {
        Task<KnowledgePointDto> Get(Guid id);

        Task<PagedResultDto<KnowledgePointDto>> GetList(KnowledegPointSearchDto dto);

        Task<KnowledgePointDto> Create(KnowledgePointCreateDto dto);

        Task<KnowledgePointDto> Update(KnowledegPointUpdateDto dto);

        Task<bool> Delete(Guid id);


        /// <summary>
        /// 获取分类管理树状列表
        /// </summary>
        Task<List<KnowledgePointDto>> GetTreeList(Guid? parentId);

        Task<List<KnowledgePointDto>> GetByParentId(Guid? parentId);

        Task<List<KnowledgePointDto>> GetParentsByIds(List<Guid> ids);
    }
}
