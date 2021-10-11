using SnAbp.Schedule.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Schedule.IServices
{
    public interface IScheduleDiaryAppService: IApplicationService
    {
        Task<DiaryDto> Get(DiarySimpleDto input);
        Task<DiaryDto> Create(DiaryCreateDto input);
        Task<DiaryDto> Update(DiaryUpdateDto input);
        Task<bool> Delete(Guid id);
        Task<Stream> Export(DiaryExportSearchDto input);
        Task<List<DiaryLogStatisticsDto>> GetLogStatistics(DiaryLogStatisticsSearchDto input);
        Task<PagedResultDto<ApprovalMultipleDto>> GetList(ApprovalSearchDto input);
        /// <summary>
        /// 获取班前讲话视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SpeachVideoDto>> GetSpeachVideo(SpeachSearchDto input);
    }
}
