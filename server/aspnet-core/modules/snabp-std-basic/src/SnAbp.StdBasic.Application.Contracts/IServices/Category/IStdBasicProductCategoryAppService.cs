using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.Utils.DataImport;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices.Category {
    public interface IStdBasicProductCategoryAppService : IApplicationService {
        /// <summary>
        /// 获取单个产品分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductCategoryDto> Get (Guid id);

        /// <summary>
        /// 通过 id 获取端子符号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetTerminalSymbolById (Guid id);

        /// <summary>
        /// 通过 code 获取端子符号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetTerminalSymbolByCode (string code);

        /// <summary>
        /// 根据输入条件按需加载数据接口
        /// </summary>
        Task<PagedResultDto<ProductCategoryDto>> GetList (ProductCategoryGetListByIdsDto input);
        /// <summary>
        /// 根据id获得当前分类中的最大编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductCategoryDto> GetListCode(Guid? id);
        /// <summary>
        /// 根据ids获得当前对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProductCategoryDto>> GetListProduct(ProductCategoryGetListByIdsDto input);
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ProductCategoryDto> Create (ProductCategoryCreateDto input);
        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ProductCategoryDto> Update (ProductCategoryUpdateDto input);
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete (Guid id);

        /// <summary>
        /// 产品excel
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        Task Upload ([FromForm] ImportData input);

        /// <summary>
        /// 产品分类导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> Export(ProductCategoryData input);
    }
}