using SnAbp.Resource.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices
{
    public interface IResourceTerminalAppService : IApplicationService
    {
        /// <summary>
        /// 根据设备 Id 获取端子列表
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<List<TerminalDto>> GetListByEquipmentId(Guid equipmentId);
    }
}
