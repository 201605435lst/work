using SnAbp.Safe.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Safe.IServices
{
 public interface ISafeProblemAppService : IApplicationService
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SafeProblemDto> Get(Guid id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SafeProblemDto>> GetList(SafeProblemSearchDto input);
        /// <summary>
        /// 获取安全问题报告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SafeProblemReportDto> GetSafeProblemReport(Guid id);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<List<SafeProblemRecordDto>> GetRecordList(Guid? id);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SafeProblemDto> Create(SafeProblemCreateDto input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SafeProblemRecordDto> CreateRecord(SafeProblemRecordCreateDto input);


        ///// <summary>
        ///// 编辑
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //Task<SafeProblemDto> Update(SafeProblemUpdateDto input);

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

