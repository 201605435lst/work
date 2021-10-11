using Microsoft.AspNetCore.Mvc;
using SnAbp.Resource.Dtos;
using SnAbp.StdBasic.Dtos;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices.Equipment
{
    public interface IResourceEquipmentPropertyAppService : IApplicationService
    {
        /// <summary>
        /// 根据设备 Id 查询属性信息
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<List<EquipmentPropertyDto>> GetList(Guid equipmentId);

        /// <summary>
        /// 添加设备扩展属性
        /// </summary>
        Task<bool> Create(EquipmentPropertyCreateDto input);

        /// <summary>
        /// 编辑设备扩展属性
        /// </summary>
        Task<bool> Update(EquipmentPropertyUpdateDto input);

        /// <summary>
        /// 删除设备扩展属性
        /// </summary>
        Task<bool> Delete(Guid id);
    }
}
