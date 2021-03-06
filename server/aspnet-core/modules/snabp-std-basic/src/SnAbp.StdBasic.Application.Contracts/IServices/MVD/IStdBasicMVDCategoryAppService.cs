using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices.Model.ModelMVD
{
    public interface IStdBasicMVDCategoryAppService : IApplicationService
    {

        /// <summary>
        /// 获得信息交换模板分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MVDCategoryDto> Get(Guid id);

        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<MVDCategoryDto>> GetList(MVDSearchDto input);
        Task<PagedResultDto<MVDCategoryDto>> GetListTree(MVDSearchDto input);
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MVDCategoryDto> Create(MVDCategoryCreateDto input);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MVDCategoryDto> Update(MVDCategoryDto input);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Import([FromForm] ImportData input);

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> Export(MVDData input);
    }
}
