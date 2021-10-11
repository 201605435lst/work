using Microsoft.AspNetCore.Mvc;
using SnAbp.Basic.Dtos;
using SnAbp.Basic.Dtos.Import;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.IO;
using SnAbp.Basic.Entities;

namespace SnAbp.Basic.IServices
{
    public interface IBasicInstallationSiteAppService : IApplicationService
    {
        /// <summary>
        /// 获得数据对象
        /// </summary>
        Task<InstallationSiteDto> Get(Guid id);

        /// <summary>
        /// 获得所有集合(为前端下拉框提供数据源)
        /// </summary>
        Task<PagedResultDto<InstallationSiteDto>> GetTreeList(InstallationSiteSearchDto input);

        /// <summary>
        /// 获取机房级联选择框初始化数据  线-站-机房
        /// </summary>
        /// <returns></returns>
        Task<List<InstallationSiteCascaderDto>> GetCascaderList();

        /// <summary>
        /// 获取机房级联选择框初始化数据  组织机构-机房
        /// </summary>
        /// <param name="orgId">组织机构id</param>
        /// <returns></returns>
        Task<List<InstallationSiteCascaderDto>> GetCascaderListWithOrg(Guid? orgId);

        /// <summary>
        /// 获取机房级联选择框根级数据  线-站-机房
        /// </summary>
        /// <returns></returns>
        Task<List<InstallationSiteCascaderDto>> GetRootCascaderList();

        /// <summary>
        /// 获取机房级联选择框指定结点的下级数据  线-站-机房
        /// </summary>
        /// <param name="id">结点id</param>
        /// <param name="type">结点类型（线0 站1 机房3 其他4）</param>
        /// <returns></returns>
        Task<List<InstallationSiteCascaderDto>> GetCascaderListByParent(Guid id, int type);

        /// <summary>
        /// 添加操作
        /// </summary>
        Task<bool> Create(InstallationSiteCreateDto input);

        /// <summary>
        /// 更新操作
        /// </summary>
        Task<bool> Update(InstallationSiteUpdateDto input);

        /// <summary>
        /// 删除操作
        /// </summary>
        Task<bool> Delete(Guid id);

        /// <summary>
        /// 上传导入文件
        /// </summary>
        Task Upload([FromForm] ImportData input);

        ///// <summary>
        ///// 获得所有集合(为前端下拉框提供数据源)
        ///// </summary>
        //Task<GetListByScopeOutDto> GetListWithOrg();

        /// <summary>
        /// 获得所有集合(为前端下拉框提供数据源)
        /// </summary>
        Task<List<GetListByScopeOutDto>> GetListByScope(GetListByScopeInputDto input);
        ///// <summary>
        ///// 获得所有集合(为前端下拉框提供数据源)
        ///// </summary>
        //Task<List<InstallationSiteDto>> GetListById(Guid id);

        Task<Stream> Export(InstallationExportData input);

    }
}
