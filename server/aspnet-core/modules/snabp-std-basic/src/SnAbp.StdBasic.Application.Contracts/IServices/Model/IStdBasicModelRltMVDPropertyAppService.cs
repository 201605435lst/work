using SnAbp.StdBasic.Dtos.Model;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices.Model.ModelMVD
{
    public interface IStdBasicModelRltMVDPropertyAppService : IApplicationService
    {
        Task<PagedResultDto<ModelRltMVDPropertyDto>> EditList(ModelRltMVDPropertyCreateDto input);

        Task<PagedResultDto<ModelRltMVDPropertyDto>> GetListByModelId(ModelRltMVDPropertySearchDto input);
    }
}
