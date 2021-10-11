using SnAbp.Quality.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Quality.IServices
{
    public interface IQualityProblemAppService : IApplicationService
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualityProblemDto> Get(Guid id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<QualityProblemDto>> GetList(QualityProblemSearchDto input);

        /// <summary>
        /// 获取记录列表
        /// </summary>
        /// <returns></returns>
        Task<List<QualityProblemRecordDto>> GetRecordList(Guid id);

        /// <summary>
        /// 获取安全问题报告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QualityProblemReportDto> GetQualityProblemReport(Guid id);
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QualityProblemDto> Create(QualityProblemCreateDto input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QualityProblemRecordDto> CreateRecord(QualityProblemRecordCreateDto input);


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QualityProblemDto> Update(QualityProblemUpdateDto input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Stream> Export(Guid id);
    }
}
