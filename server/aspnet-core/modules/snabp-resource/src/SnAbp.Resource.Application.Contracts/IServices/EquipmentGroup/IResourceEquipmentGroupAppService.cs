using Microsoft.AspNetCore.Mvc;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Dtos.Export;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices.EquipmentGroup
{
   public interface IResourceEquipmentGroupAppService : IApplicationService
    
    {
        /// <summary>
        /// 获取设备分组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquipmentGroupDto> Get(Guid id);
        /// 根据条件获取设备分组
        /// </summary>
        /// <param name="parentId">构件分类Id</param>
        /// <returns></returns>
        Task<PagedResultDto<EquipmentGroupDto>> GetList(EquipmentGroupGetListDto input);
        /// <summary>
        /// 创建设备分组信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EquipmentGroupDto> Create(EquipmentGroupCreateDto input);
        /// <summary>
        /// 导出设备分组信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task upload([FromForm] DataImportDto input);
        /// <summary>
        /// 导出设备数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> Export(EquipmentGroupData input);
        /// <summary>
        /// 修改设备分组信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EquipmentGroupDto> Update(EquipmentGroupUpdateDto input);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
