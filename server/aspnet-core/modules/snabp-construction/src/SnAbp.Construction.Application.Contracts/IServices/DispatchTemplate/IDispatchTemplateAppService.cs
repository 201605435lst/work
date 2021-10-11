
using System;
using SnAbp.Construction.Dtos;
using Volo.Abp.Application.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using Volo.Abp.Application.Services;

namespace SnAbp.Construction.IServices
{
    /// <summary>
    /// 派工单模板 IService 接口
    /// </summary>
    public interface IDispatchTemplateAppService : ICrudAppService<
        DispatchTemplateDto,
        Guid,
        DispatchTemplateSearchDto,
        DispatchTemplateCreateDto,
        DispatchTemplateUpdateDto
    >
    {

        /// <summary>
        /// 设置默认模板
        /// </summary>
        Task<bool> setDefault(Guid id);
        /// <summary>
        /// 删除多个
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteRange(List<Guid> ids);
    }
}
