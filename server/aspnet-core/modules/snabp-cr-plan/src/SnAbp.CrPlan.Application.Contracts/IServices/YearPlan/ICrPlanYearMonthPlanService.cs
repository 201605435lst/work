using Microsoft.AspNetCore.Mvc;
using SnAbp.CrPlan.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using SnAbp.CrPlan.Dtos.Import;
using SnAbp.Bpm;
using SnAbp.CrPlan.Dtos;

namespace SnAbp.CrPlan.IServices
{
    /// <summary>
    /// 年月表计划
    /// </summary>
    public interface ICrPlanYearMonthPlanService : IApplicationService
    {
        /// <summary>
        /// 获得天窗类型列表
        /// </summary>
        /// <returns></returns>
        List<string> GetSkyligetTypeList();

        /// <summary>
        /// 获得年计划列表
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetListYear(YearMonthSearchDto input);

        /// <summary>
        /// 获得已提交的数量大于0的年计划列表
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetSubmitedListYear(YearMonthSearchDto input);

        /// <summary>
        /// 获得月计划列表
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetListMonth(YearMonthSearchDto input);

        /// <summary>
        /// 获得已提交的数量大于0的月计划列表
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetSubmitedListMonth(YearMonthSearchDto input);

        /// <summary>
        /// 获取统计后的年表数据 组织机构汇总
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<YearMonthPlanYearStatisticalDto>> GetYearStatisticData(YearMonthSearchDto input);

        /// <summary>
        /// 获取统计后的月表数据 组织机构汇总
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<YearMonthPlanMonthStatisticalDto>> GetMonthStatisticData(YearMonthSearchDto input);

        /// <summary>
        /// 获得可以变更的年月计划表
        /// </summary>
        Task<List<YearMonthPlanChangeDto>> GetCanChangePlanList(YearMonthGetChangeDto input);

        /// <summary>
        /// 获得当前年月表变更列表(点击变更按钮后看到的列表)
        /// </summary>
        Task<List<YearMonthPlanAlterDto>> GetOwnsChangPlan(YearMonthGetChangeDto input);

        /// <summary>
        /// 保存年月表计划要变更数据
        /// </summary>
        Task<bool> CreateChangePlan(CommonGuidListGetDto input);

        /// <summary>
        /// 添加数据(年表、月表、年度月表)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Create(YearMonthCreateDto input);

        /// <summary>
        /// 编辑天窗类型
        /// </summary>
        Task<bool> UpdateSkyligetType(YearMonthPlanUpdateDto input);

        /// <summary>
        /// 编辑年/月天窗类型
        /// </summary>
        Task<bool> UpdateYearMonthPlan(YearMonthPlanMonthUpdateDto input);

        /// <summary>
        /// 删除年月表变更数据
        /// </summary>
        Task<bool> DeletePlanAtler(CommonGuidGetDto input);

        /// <summary>
        /// 上传EXCEL文件
        /// </summary>
        Task<bool> UploadAsync([FromForm] ImportData input);

        /// <summary>
        /// 导出Ecel文件
        /// </summary>
        Task<Stream> DownLoadAsync(YearMonthExportDto input);

        /// <summary>
        /// 年月表变更导出EXCEL
        /// </summary>
        Task<Stream> DownLoadChange(YearMonthExportDto input);

        /// <summary>
        /// 年月表变更导入EXCEL
        /// </summary>
        Task<bool> UploadChange([FromForm] ImportData input);

        /// <summary>
        /// 年月表提交审核
        /// </summary>
        Task<bool> SubmitForExam(YearMonthExamDto input);

        /// <summary>
        /// 年月表(变更)计划审核驳回
        /// </summary>
        Task<bool> RejectForExam(Guid id);

        /// <summary>
        /// 年月表审核完成
        /// </summary>
        Task<bool> FinishForExam(Guid id, WorkflowState state);

        /// <summary>
        /// 年月表变更提交审核
        /// </summary>
        Task<bool> SubmitChangeForExam(YearMonthChangeExamDto input);

        /// <summary>
        /// 年月表变更完成审核
        /// </summary>
        Task<bool> FinishChangeForExam(Guid examId, WorkflowState state);

        /// <summary>
        /// 更新测试项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpgradeTestItems(RepairTestItemUpgradeDto input);

        /// <summary>
        /// 清空已填报项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> ClearFilled(YearMonthCreateDto input);

        /// <summary>
        /// 导出月表记录数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> ExportMonthStatisticData(YearMonthSearchDto input);

        /// <summary>
        /// 导出年表记录数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> ExporYearStatisticData(YearMonthSearchDto input);

        /// <summary>
        /// 统计年月表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetYearMonthTotalSimpleDto> GetTotal(GetTotalSearchDto input);

        Task<List<string>> GetOrganizationNames(YearMonthSimpleDto input);

        Task<PagedResultDto<YearMonthAlterRecordDto>> GetYearMonthAlterRecords(YearMonthAlterRecordSearchDto input);

        /// <summary>
        /// 审批通过后退回年表月度计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> BackMonthOfYearPalns(YearMonthSearchDto input);
    }
}
