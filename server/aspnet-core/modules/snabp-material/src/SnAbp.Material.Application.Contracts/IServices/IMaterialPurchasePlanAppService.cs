using SnAbp.Bpm.Dtos;
using SnAbp.Material.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Material.IServices
{
    public interface IMaterialPurchasePlanAppService : IApplicationService
    {
        Task<PurchasePlanDto> Get(Guid id);
        Task<PagedResultDto<PurchasePlanDto>> GetList(PurchasePlanSearchDto input);
        Task<PurchasePlanDto> Create(PurchasePlanCreateDto input);
        Task<PurchasePlanDto> Update(PurchasePlanUpdateDto input);
        Task<bool> Delete(Guid id);
        //Task<Stream> Export(EducePurchaseDto input);

        //Task<List<SingleFlowNodeDto>> GetFlowInfo(Guid workflowTemplateId);

        //Task<bool> UpdateState(Guid id);

        //Task<List<SingleFlowNodeDto>> GetRunFlowInfo(Guid workFlowId, Guid purchaseId);

        ///// <summary>
        ///// 查询量差分析
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //Task<PagedResultDto<GapAnalysisSearchDto>> GetGapAnalysis(PurchaseSearchDto input);
    }
}
