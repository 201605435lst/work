using SnAbp.Resource.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices.Equipment
{
    public interface IResourceCableExtendAppService : IApplicationService
    {
        /// <summary>
        /// 获得实体对象
        /// </summary>
        Task<CableExtendDto> Get(Guid id);

        /// <summary>
        /// 更新操作
        /// </summary>
        Task<bool> Update(CableExtendDto input);
    }
}
