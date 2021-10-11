using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicComponentCategoryAppService : IApplicationService
    {
        /// <summary>
        /// 获取单个构件分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ComponentCategoryDto> Get(Guid id);
        /// <summary>
        /// 获取单个构件分类信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ComponentCategoryDto> GetByCode(string code);
        /// <summary>
        /// 根据输入条件获取构件分类树
        /// </summary>
        /// <param name="input">输入条件</param>
        /// <returns></returns>
        Task<PagedResultDto<ComponentCategoryDto>> GetList(ComponentCategoryGetListByIdsDto input);
        /// <summary>
        /// 根据id获得当前分类中的最大编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ComponentCategoryDto> GetListCode(Guid? id);
        /// <summary>
        /// 根据ids获得当前对象
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<PagedResultDto<ComponentCategoryDto>> GetListComponent(ComponentCategoryGetListByIdsDto input);

        /// <summary>
        /// 构件站点excel
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        Task Upload([FromForm] ImportData input);
        /// <summary>
        /// 创建构建分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ComponentCategoryDto> Create(ComponentCategoryCreateDto input);
        /// <summary>
        /// 修改构建分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ComponentCategoryDto> Update(ComponentCategoryUpdateDto input);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        /// <summary>
        /// 构件分类导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> Export(ComponentCategoryData input);
    }
}
