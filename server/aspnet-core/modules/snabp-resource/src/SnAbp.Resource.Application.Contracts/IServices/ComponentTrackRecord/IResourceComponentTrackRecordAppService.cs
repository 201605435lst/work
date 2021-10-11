using SnAbp.Resource.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices
{
    public interface IResourceComponentTrackRecordAppService : IApplicationService
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ComponentTrackRecordDto>> Get(Guid id);

        /// <summary>
        /// 通过安装设备获取构件跟踪记录
        /// </summary>
        /// <param name="equpmentId"></param>
        /// <returns></returns>
        Task<List<ComponentTrackRecordDto>> GetByInstallationEquipmentId(Guid installationEquipmentId);


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ComponentTrackRecordDto> Create(ComponentTrackRecordCreateDto input);

        /// <summary>
        /// 根据二维码查询安装设备id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Guid?> GetInstallationEquipmentId(string QRCode);
    }
}

