using SnAbp.Resource.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices.Equipment
{
    public interface IResourceCableCoreAppService : IApplicationService
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="cableId"></param>
        /// <returns></returns>
        Task<List<CableCoreDto>> GetList(Guid cableId);

        /// <summary>
        /// 更新业务类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateTerminalLink(TerminalLinkUpdateDto input);
        /// <summary>
        /// 更新线芯类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateCableCore(CableCoreUpdateDto input);
    }
}
