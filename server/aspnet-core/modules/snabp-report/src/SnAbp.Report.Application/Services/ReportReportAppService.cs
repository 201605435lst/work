using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Identity;
using SnAbp.Message;
using SnAbp.Message.Notice;
using SnAbp.Report.Dtos;
using SnAbp.Report.Entities;
using SnAbp.Report.Enums;
using SnAbp.Report.IServices;
using SnAbp.Report.Permissions;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace SnAbp.Report.Services
{
    [Authorize]
    public class ReportReportAppService : ReportAppService, IReportReportAppService
    {
        private IMessageNoticeProvider _iMessageNoticeProvider;
        private IdentityUserManager _userManagerRepositor;
        private readonly IRepository<ReportRltUser, Guid> _repositoryReportRltUser;
        private readonly IRepository<ReportRltFile, Guid> _repositoryReportRltFiles;
        private readonly IRepository<Report, Guid> _repositoryReport;
        private readonly IGuidGenerator _guidGenerate;
        protected IIdentityUserRepository _userRepository { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrganizationRepository _organizationRepository;
        public ReportReportAppService(
                IMessageNoticeProvider iMessageNoticeProvider,
                IRepository<ReportRltUser, Guid> repositoryReportRltUser,
                IRepository<ReportRltFile, Guid> repositoryReportRltFiles,
                 IRepository<Report, Guid> repositoryReport,
                 IOrganizationRepository organizationRepository,
                IHttpContextAccessor httpContextAccessor,
                IIdentityUserRepository userRepository,
                IdentityUserManager userManagerRepository,
                IGuidGenerator guidGenerate
            )
        {
            _iMessageNoticeProvider = iMessageNoticeProvider;
            _userRepository = userRepository;
            _userManagerRepositor = userManagerRepository;
            _repositoryReportRltUser = repositoryReportRltUser;
            _repositoryReportRltFiles = repositoryReportRltFiles;
            _repositoryReport = repositoryReport;
            _guidGenerate = guidGenerate;
            _httpContextAccessor = httpContextAccessor;
            _organizationRepository = organizationRepository;
        }
        /// <summary>
        /// 获取单个汇报表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ReportDto> Get(Guid id)
        {

            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的汇报表");
            var report = _repositoryReport.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (report == null) throw new UserFriendlyException("当前汇报表不存在");
            var result = ObjectMapper.Map<Report, ReportDto>(report);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取汇报列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ReportDto>> GetList(ReportSearchDto input)
        {
            var result = new PagedResultDto<ReportDto>();
            var report = _repositoryReport.WithDetails()
                .WhereIf(input.ReportsType == "send", x => x.CreatorId == CurrentUser.Id.Value)
                .WhereIf(input.ReportsType == "receive", x => x.ReportRltUsers.Any(x => x.UserId == CurrentUser.Id))
                .WhereIf(!String.IsNullOrEmpty(input.KeyWords), x => x.Title.Contains(input.KeyWords))
                .WhereIf(input.Type.IsIn(ReportType.DayRepot, ReportType.MonthReport, ReportType.WeekReport), x => x.Type == input.Type);

            var res = ObjectMapper.Map<List<Report>, List<ReportDto>>(report.OrderBy(x => x.Date).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            foreach (var item in res)
            {
                var user = await _userRepository.GetAsync(item.CreatorId.GetValueOrDefault());
                item.CreatorName = user.Name;
            }
            result.Items = res;
            result.TotalCount = report.Count();
            return result;
        }
        /// <summary>
        /// 创建一个工作汇报
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ReportPermissions.Report.Create)]
        public async Task<ReportDto> Create(ReportCreateDto input)
        {
            ReportDto reportDto = new ReportDto();
            var report = new Report();
            CheckSameTitle(input.Type, input.Title, null);
            var isSystemUser = await _userManagerRepositor.isSystem(CurrentUser.Id.Value);
            if (!isSystemUser)
            {
                // 获取当前用户所在的组织机构
                var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
                var organization = !string.IsNullOrEmpty(organizationIdString) ? _organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
                if (organization != null)
                {
                    input.OrganizationId = organization.Id;
                }
            }
            else
            {
                input.OrganizationId = null;

            }


            ObjectMapper.Map(input, report);
            report.SetId(_guidGenerate.Create());
            ///保存附件
            report.ReportRltFiles = new List<ReportRltFile>();
            foreach (var reportRltFiles in input.ReportRltFiles)
            {
                report.ReportRltFiles.Add(
                    new ReportRltFile(_guidGenerate.Create())
                    {
                        FileId = reportRltFiles.FileId,
                    });
            }
            //保存通知人员
            report.ReportRltUsers = new List<ReportRltUser>();
            foreach (var reportRltUsers in input.ReportRltUsers)
            {
                report.ReportRltUsers.Add(
                    new ReportRltUser(_guidGenerate.Create())
                    {
                        UserId = reportRltUsers.UserId,
                    });
            }
            ///保存成功后给通知人员发送消息
            var message = new NoticeMessage();
            message.SendType = Message.MessageDefine.SendModeType.User;
            // 获取通知人员的ids
            var userIds = report.ReportRltUsers.Select(x => x.UserId);
            message.SetUserIds(userIds.ToArray());
            var content = new NoticeMessageContent();
            content.Type = "report";
            var reportMessageNoticeDto = new ReportMessageNoticeDto();
            //获取提交消息的人
            var user = await _userRepository.GetAsync(CurrentUser.Id.Value);
            reportMessageNoticeDto.name = user.Name;
            var reportTypeName = getReportTypeName(input.Type);
            reportMessageNoticeDto.reportType = reportTypeName;
            content.CreateContent(reportMessageNoticeDto);
            message.SetContent(content);
            await _iMessageNoticeProvider.PushAsync(message.GetBinary());
            await _repositoryReport.InsertAsync(report);
            reportDto = ObjectMapper.Map<Report, ReportDto>(report);
            return reportDto;
        }
        /// <summary>
        /// 修改一个工作汇报
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ReportPermissions.Report.Update)]
        public async Task<ReportDto> Update(ReportUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的汇报");
            var report = await _repositoryReport.GetAsync(input.Id);
            if (report == null) throw new UserFriendlyException("当前汇报不存在");
            CheckSameTitle(input.Type, input.Title, input.Id);
            input.OrganizationId = report.OrganizationId;
            //清除保存的附件关联表信息
            await _repositoryReportRltFiles.DeleteAsync(x => x.ReportId == input.Id);
            //清除保存的通知人员关联表信息
            await _repositoryReportRltFiles.DeleteAsync(x => x.ReportId == input.Id);
            ObjectMapper.Map(input, report);
            //report.SetId(input.Id);
            //重新保存关联表信息
            report.ReportRltFiles = new List<ReportRltFile>();
            foreach (var reportRltFiles in input.ReportRltFiles)
            {
                report.ReportRltFiles.Add(
                    new ReportRltFile(_guidGenerate.Create())
                    {
                        FileId = reportRltFiles.FileId,
                    });
            }
            //保存通知人员
            report.ReportRltUsers = new List<ReportRltUser>();
            foreach (var reportRltUsers in input.ReportRltUsers)
            {
                report.ReportRltUsers.Add(
                    new ReportRltUser(_guidGenerate.Create())
                    {
                        UserId = reportRltUsers.UserId,
                    });
            }
            await _repositoryReport.UpdateAsync(report);
            return ObjectMapper.Map<Report, ReportDto>(report);
        }
        /// <summary>
        /// 导出Ecel文件
        /// </summary>
        [Authorize(ReportPermissions.Report.Export)]
        [Produces("application/octet-stream")]
        public Task<Stream> Export(ReportExportDto input)
        {
            Stream stream = null;
            byte[] sbuf;
            var dataTable = (DataTable)null;//表
            var dataColumn = (DataColumn)null;//行
            var dataRow = (DataRow)null;//列
                                        //获取需要导出的数据
            var report = _repositoryReport.WithDetails()
                 .WhereIf(!String.IsNullOrEmpty(input.KeyWords), x => x.Title.Contains(input.KeyWords))
                 .WhereIf(input.ReportsType == "send", x => x.CreatorId == CurrentUser.Id.Value)
                .WhereIf(input.ReportsType == "receive", x => x.ReportRltUsers.Any(x => x.UserId == CurrentUser.Id))
                .WhereIf(input.Type.IsIn(ReportType.DayRepot, ReportType.MonthReport, ReportType.WeekReport), x => x.Type == input.Type)
                         .WhereIf(input.Ids.Count > 0, x => input.Ids.Contains(x.Id));
            var list = ObjectMapper.Map<List<Report>, List<ReportDto>>(report.OrderBy(x => x.Date).ToList());
            if (list.Count == 0) throw new UserFriendlyException("未找到任何导出数据");
            //表格初始化
            dataTable = new DataTable();
            var enumValues = Enum.GetValues(typeof(Enums.ReportExcelCol));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    dataColumn = new DataColumn(Enum.GetName(typeof(Enums.ReportExcelCol), item));
                    dataTable.Columns.Add(dataColumn);
                }
            }
            var index = 0;
            //表格添加数据
            foreach (var item in list)
            {
                dataRow = dataTable.NewRow();
                dataRow[ReportExcelCol.序号.ToString()] = index;
                dataRow[ReportExcelCol.项目名称.ToString()] = item.Project.Name;
                dataRow[ReportExcelCol.标题.ToString()] = item.Title;
                dataRow[ReportExcelCol.类型.ToString()] = getReportTypeName(item.Type);
                dataRow[ReportExcelCol.日期.ToString()] = item.Date.ToString("yyyy-MM-dd");
                dataRow[ReportExcelCol.工作计划.ToString()] = item.Plan;
                dataRow[ReportExcelCol.工作记录.ToString()] = item.WorkRecord;
                var userName = "";
                if (item.ReportRltUsers.Count > 0)
                {
                    foreach (var user in item.ReportRltUsers)
                    {
                        userName += userName == "" ? user.User.Name : "、" + userName;
                    }
                }
                dataRow[ReportExcelCol.通知人.ToString()] = userName;
                dataTable.Rows.Add(dataRow);
                index++;
            }
            sbuf = ExcelHelper.DataTableToExcel(dataTable, "项目工作汇报表.xlsx", null, "项目工作汇报记录");
            stream = new MemoryStream(sbuf);
            return Task.FromResult(stream);
        }
        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(ReportPermissions.Report.Delete)]
        public async Task<bool> Delete(List<Guid> ids)
        {
            await _repositoryReport.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }
        #region
        public string getReportTypeName(ReportType Type)
        {
            string typeName = null;

            if (Type == ReportType.DayRepot)
            {
                typeName = "日报";
            }
            if (Type == ReportType.WeekReport)
            {
                typeName = "周报";
            }
            if (Type == ReportType.MonthReport)
            {
                typeName = "月报";
            }
            return typeName;
        }
        private bool CheckSameTitle(ReportType Type, string Title, Guid? Id)
        {
            var contract = _repositoryReport.Where(x => x.Type == Type && x.Title == Title).WhereIf(Id != null && Id != Guid.Empty, x => x.Id != Id);
            if (contract.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("当前报告标题已存在！！！");
            }
            return true;
        }
        #endregion
    }
}
