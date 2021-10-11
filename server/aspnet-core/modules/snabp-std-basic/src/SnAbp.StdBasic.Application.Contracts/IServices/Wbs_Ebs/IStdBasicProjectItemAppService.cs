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
    public interface IStdBasicProjectItemAppService : IApplicationService
    {
        /// <summary>
        /// 获取工程工项列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns></returns>
        Task<PagedResultDto<ProjectItemDto>> GetList(ProjectItemSearchDto  input);

        Task<PagedResultDto<ProjectItemDto>> GetListProjectItem(ProjectItemSearchDto input);
        /// <summary>
        /// 根据Id获取工程工项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectItemDto> Get(Guid id);
        /// <summary>
        /// 添加工程工项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ProjectItemDto> Create(ProjectItemCreateDto input);
        /// <summary>
        /// 修改工程工项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ProjectItemDto> Update(ProjectItemDto input);

        /// <summary>
        /// 删除工程工项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        Task Upload([FromForm] ImportData input);

        Task<Stream> Export(ProjectItemData input);
    }
}
