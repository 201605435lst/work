using SnAbp.Regulation.Dtos.Label;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Regulation.IServices.Label
{
    public interface IRegulationLabelAppService:IApplicationService
    {
        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<LabelDto> Create(LabelCreateDto input);
        
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(List<Guid> ids);

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<LabelDto> Update(LabelUpdateDto input);

        /// <summary>
        /// <summary>
        /// 查询标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<LabelDto>> GetList(LabelSearchDto input);
        Task<LabelDto> Get(Guid id);

    }
}
