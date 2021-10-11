using SnAbp.Resource.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices
{
    public interface IResourceTerminalLinkAppService : IApplicationService
    {
        /// <summary>
        /// 根据端子 Id 获取连接关系列表
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        Task<List<TerminalLinkDto>> GetListByTerminalId(Guid terminalId);


        /// <summary>
        /// 根据设备 Id 获取连接关系列表
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<List<TerminalLinkDto>> GetListByEquipmentId(Guid equipmentId);
    }
}
