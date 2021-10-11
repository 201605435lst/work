using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
   public interface IStdBasicQuotaCategoryAppService : IApplicationService
    {
        /// <summary>
        /// 获取单个定额分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QuotaCategoryDto> Get(Guid id);

        /// <summary>
        /// 根据输入条件获取定额分类树
        /// </summary>
        /// <param name="input">输入条件</param>
        /// <returns></returns>
        Task<PagedResultDto<QuotaCategoryDto>> GetList(QuotaCategoryGetListByIdsDto input);

        Task<PagedResultDto<QuotaCategoryTreeDto>> GetListTree(QuotaCategoryGetListByIdsDto input);

        /// <summary>
        /// 根据id获得当前分类中的最大编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QuotaCategoryDto> GetListCode(Guid? id);
        /// <summary>
        /// 创建定额分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QuotaCategoryDto> Create(QuotaCategoryCreateDto input);
        /// <summary>
        /// 修改定额分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QuotaCategoryDto> Update(QuotaCategoryUpdateDto input);
        /// <summary>
        /// 删除定额分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        Task Upload([FromForm] ImportData input);
        Task<Stream> Export(QuotaCategoryData input);
    }
}
