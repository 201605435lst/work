using Microsoft.AspNetCore.Mvc;
using SnAbp.Common.Dtos.Task;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Dtos.Export;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices.Equipment
{
    public interface IResourceEquipmentAppService : IApplicationService
    {
        /// <summary>
        /// 获得实体对象
        /// </summary>
        Task<EquipmentDetailDto> Get(Guid id);

        /// <summary>
        /// 获取GisData
        /// </summary>
        Task<string> GetGisData(GetGisDataInputDto input);

        /// <summary>
        /// 根据ids获得实体对象
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<PagedResultDto<EquipmentMiniDto>> GetByIds(List<Guid> ids);

        /// <summary>
        /// 获得实体对象
        /// </summary>
        Task<EquipmentDetailDto> GetByGroupNameAndName(string groupName, string name);

        /// <summary>
        /// 获得设备关联文件列表
        /// </summary>
        Task<List<EquipmentRltFileDto>> GetFileList(Guid equipmentId);

        /// <summary>
        /// 获得分页集合
        /// </summary>
        Task<EquipmentGetListDto> GetList(EquipmentSearchDto input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<EquipmentMiniDto>> GetSimpleList(EquipmentSimpleSearchDto input);

        /// <summary>
        /// 获取安装位置下的设备构建分类，按需逐级加载
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<GetListByScopeOutput>> SearchListByScopeCode(SearchListByScopeInput input);

        /// <summary>
        /// 获取一组设备的范围
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<GetScopesByGroupAndNameOut>> GetScopesByGroupAndName(GetScopesByGroupAndNameInput input);

        /// <summary>
        /// 获取一个范围的设备分组列表
        /// </summary>
        /// <param name="scopeCode"></param>
        /// <returns></returns>
        Task<List<string>> GetEquipmentGroupsByScopeCode(string scopeCode);

        /// <summary>
        /// 添加操作
        /// </summary>
        Task<bool> Create(EquipmentCreateDto input);

        /// <summary>
        /// 添加设备关联文件
        /// </summary>
        Task<bool> CreateFile(EquipmentRltFileCreateDto input);

        /// <summary>
        /// 更新操作
        /// </summary>
        Task<bool> Update(EquipmentUpdateDto input);

        /// <summary>
        /// 生成工程量
        /// </summary>
        /// <returns></returns>
        Task GenerateQuantity(string taskKey);

        /// <summary>
        /// 更新Gis数据
        /// </summary>
        Task<bool> UpdateGisData(UpdateGisDataInput input);

        /// <summary>
        /// 数据导入
        /// </summary>
        Task<string> Upload([FromForm] ImportData input);

        /// <summary>
        /// 工程数据导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task EngineeringDataImport([FromForm] DataImportDto input);

        /// <summary>
        /// 删除操作
        /// </summary>
        Task<bool> Delete(Guid id);

        /// <summary>
        /// 删除设备关联文件
        /// </summary>
        Task<bool> DeleteFile(Guid id);

        /// <summary>
        /// 导出设备数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> Export(EquipmentData input);


        Task<List<EquipmentSimpleDto>> GetEquipment(List<Guid> ids);

        Task<bool> OneTouchInStorage();

    }
}
