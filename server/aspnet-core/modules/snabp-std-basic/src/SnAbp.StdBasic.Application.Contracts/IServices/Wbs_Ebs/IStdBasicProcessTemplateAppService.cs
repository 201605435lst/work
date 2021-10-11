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
    /// <summary>
    /// 工序模板
    /// </summary>
    public interface IStdBasicProcessTemplateAppService : IApplicationService
    {
        /// <summary>
        /// 获取单个工序模板信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcessTemplateDto> Get(Guid id);

        /// <summary>
        /// 根据输入条件获取工序模板树
        /// </summary>
        /// <param name="input">输入条件</param>
        /// <returns></returns>
        Task<PagedResultDto<ProcessTemplateDto>> GetList(ProcessTemplateGetListByIdsDto input);

        /// <summary>
        /// 根据id获得当前工序模板中的最大编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcessTemplateDto> GetListCode(Guid? id);


        /// <summary>
        /// 创建工序模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ProcessTemplateDto> Create(ProcessTemplateCreateDto input);
        /// <summary>
        /// 修改工序模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ProcessTemplateDto> Update(ProcessTemplateUpdateDto input);
        /// <summary>
        /// 删除工序模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        Task Upload([FromForm] ImportData input);
        Task<Stream> Export(ProcessTemplateData input);

    }
}
