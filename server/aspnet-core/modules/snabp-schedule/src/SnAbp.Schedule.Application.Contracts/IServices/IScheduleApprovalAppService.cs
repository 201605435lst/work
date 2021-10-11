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
    public interface IScheduleApprovalAppService : IApplicationService
    {
        Task<ApprovalDto> Create(ApprovalCreateDto input);
        Task<bool> Delete(Guid id);
        Task<ApprovalDto> Update(ApprovalUpdateDto input);
        Task<PagedResultDto<ApprovalDto>> GetList(ApprovalSearchDto input);
        Task<ApprovalDto> Get(Guid id);
        Task<string> GetCode();
        //Task Import([FromForm] ImportData input);
        Task<Stream> Export(EduceApprovalDto input);
    }
}
