using SnAbp.Bpm;
using SnAbp.CrPlan.Dto.AlterRecord;
using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.CrPlan.IServices.AlterRecord
{
    public interface ICrPlanAlterRecordAppService : IApplicationService
    {
        Task<AlterRecordDetailDto> Get(CommonGuidGetDto input);

        Task<PagedResultDto<AlterRecordSimpleDto>> GetList(AlterRecordSearchDto input);

        /// <summary>
        /// 获取待选计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task<PagedResultDto<DailyPlanSelectableDto>> GetSelectablePlans(SelectablePlansSearchDto input);

        /// <summary>
        /// 获取单条待选计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DailyPlanSelectableDto> GetSelectablePlan(CommonGuidGetDto input);


        /// <summary>
        /// 获取多条待选计划
        /// </summary>
        /// <param name="ids">DailyPlan ids</param>
        /// <returns></returns>
        Task<List<DailyPlanSelectableDto>> ForSelectablePlanByIds(AlterRecordGetListDto input);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> Export(AlterRecordExportInputDto input);

        Task<AlterRecordDto> Create(AlterRecordCreateDto input);

        Task<AlterRecordDto> Update(AlterRecordUpdateDto input);

        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> SubmitForExam(CommonGuidGetDto input);

        /// <summary>
        /// 变更计划审批
        /// </summary>
        /// <param name="id">审批流程id</param>
        /// <param name="state">审批状态</param>
        /// <returns></returns>
        Task<bool> ApprovePlanAlter(Guid id, WorkflowState state);

        Task<bool> Delete(CommonGuidGetDto input);

        //Task<List<Guid>> GetSelectableAllPlans(SelectablePlansSearchDto input);
    }
}
