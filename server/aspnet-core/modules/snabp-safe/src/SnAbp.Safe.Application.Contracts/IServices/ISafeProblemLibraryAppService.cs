/**********************************************************************
*******命名空间： SnAbp.Safe.IServices
*******接口名称： ISafeProblemLibraryAppService
*******接口说明： 问题库服务
*******作    者： 东腾 Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/7 15:34:15
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2019-2020. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Safe.Dtos;
using SnAbp.Utils.DataImport;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Safe.IServices
{
    public interface ISafeProblemLibraryAppService : IApplicationService
    {

        Task<Stream> Download(List<Guid> ids);
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SafeProblemLibraryDto> Get(Guid id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SafeProblemLibraryDto>> GetList(SafeProblemLibrarySearchDto input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SafeProblemLibraryDto> Create(SafeProblemLibraryCreateDto input);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SafeProblemLibraryDto> Update(SafeProblemLibraryUpdateDto input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Upload(SafeProblemFileUploadDto input);
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> Export(SafeProblemLibraryExportDto input);
    }
}
