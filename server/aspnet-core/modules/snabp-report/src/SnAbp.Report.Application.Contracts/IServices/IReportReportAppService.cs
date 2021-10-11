using SnAbp.Report.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Report.IServices
{
    public interface IReportReportAppService : IApplicationService
    {
        /// <summary>
        /// 获取单个汇报表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ReportDto> Get(Guid id);
        /// <summary>
        /// 获取汇报列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ReportDto>> GetList(ReportSearchDto input);
        /// <summary>
        /// 创建一个工作汇报
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ReportDto> Create(ReportCreateDto input);
        /// <summary>
        /// 修改一个工作汇报
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ReportDto> Update(ReportUpdateDto input);
        /// <summary>
        /// 导出Ecel文件
        /// </summary>
        Task<Stream> Export(ReportExportDto input);
        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(List<Guid> ids);
    }
}

 
