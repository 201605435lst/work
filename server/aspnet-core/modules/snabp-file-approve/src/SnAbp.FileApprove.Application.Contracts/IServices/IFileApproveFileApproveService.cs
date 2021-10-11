using SnAbp.File.Dtos;
using SnAbp.FileApprove.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

/************************************************************************************
*命名空间：SnAbp.FileApprove.IServices
*文件名：IFileApproveFileApproveService
*创建人： liushengtao
*创建时间：2021/9/1 8:53:34
*描述：
*
***********************************************************************/
namespace SnAbp.FileApprove.IServices
{
   public interface IFileApproveFileApproveService : IApplicationService
    {
        Task<FileApproveDto> Create(FileApproveSimpleDto input);
        Task<FileApproveDto> Update(FileApproveSimpleDto input);
        Task<FileApproveDto> Get(Guid id);
        Task<bool> Process(FileApproveProcessDto input);
        Task<PagedResultDto<FileRltFileApproveDto>> GetResourceList(ResourceInputDto input);
    }
}
