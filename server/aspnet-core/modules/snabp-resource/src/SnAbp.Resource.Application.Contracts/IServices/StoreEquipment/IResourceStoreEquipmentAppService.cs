using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices
{
    public interface IResourceStoreEquipmentAppService : IApplicationService
    {
        /// <summary>
        /// 批量创建库存设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<StoreEquipmentDto>> Create(List<StoreEquipmentCreateDto> input);

        /// <summary>
        /// 根据条件获取库存设备集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<StoreEquipmentDto>> GetList(StoreEquipmentSearchDto input);
        /// <summary>
        /// 通过code集合来获取对应的库存设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<StoreEquipmentDto>> GetListByCode(StoreEquipmentSearchSimpleDto input);
        /// <summary>
        /// 通过ids数组来获取相应的库存设备信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<PagedResultDto<StoreEquipmentDto>> GetListByIds(List<Guid> ids);
        /// <summary>
        /// 获取设备履历
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<StoreEquipmentDto> GetStoreEquipmentRecords(Guid id);
        /// <summary>
        /// 将数据导出到excel表格中
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<Stream> ExportStoreEquipments(List<StoreEquipmentDto> input);
    }
}
