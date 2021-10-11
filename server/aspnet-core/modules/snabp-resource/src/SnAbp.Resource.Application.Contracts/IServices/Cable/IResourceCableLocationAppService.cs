using SnAbp.Resource.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices.Equipment
{
    public interface IResourceCableLocationAppService : IApplicationService
    {
        /// <summary>
        /// 获得实体对象
        /// </summary>
        Task<CableLocationDto> Get(Guid id);

        /// <summary>
        /// 根据电缆 Id 查询电缆埋深信息
        /// </summary>
        /// <param name="cableId"></param>
        /// <returns></returns>
        Task<List<CableLocationDto>> GetList(Guid cableId);

        /// <summary>
        /// 添加操作
        /// </summary>
        Task<bool> Create(CableLocationCreateDto input);

        /// <summary>
        /// 更新操作
        /// </summary>
        Task<bool> Update(CableLocationCreateDto input);
    }
}
