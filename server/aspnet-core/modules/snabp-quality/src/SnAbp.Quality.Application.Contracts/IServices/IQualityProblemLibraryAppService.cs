using Microsoft.AspNetCore.Mvc;
using SnAbp.Quality.Dtos;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity.Dtos;

namespace SnAbp.Quality.IServices
{
    public interface IQualityProblemLibraryAppService
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualityProblemLibraryDto> Get(Guid id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<QualityProblemLibraryDto>> GetList(QualityProblemLibrarySearchDto input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QualityProblemLibraryDto> Create(QualityProblemLibraryCreateDto input);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QualityProblemLibraryDto> Update(QualityProblemLibraryUpdateDto input);

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
        Task<bool> Upload(QualityProblemFileUploadDto input);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public Task<Stream> Export(List<Guid> ids);
        Task<Stream> Export(QualityProblemLibraryExportDto input);
    }
}
