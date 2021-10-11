/**********************************************************************
*******命名空间： SnAbp.Safe.Services
*******类 名 称： SafeProblemAppService
*******类 说 明： 安全问题管理服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/11 16:16:07
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.AspNetCore.Mvc;
using NPOI.XWPF.UserModel;
using SnAbp.Identity;
using SnAbp.Message.Notice;
using SnAbp.Safe.Dtos;
using SnAbp.Safe.Entities;
using SnAbp.Safe.IServices;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Safe.Services
{
    /// <summary>
    /// $$
    /// </summary>
    public class SafeProblemAppService : SafeAppService, ISafeProblemAppService
    {
        private readonly IRepository<SafeProblem, Guid> _safeProblems;
        private readonly IRepository<SafeProblemLibrary, Guid> _safeProblemLibrary;
        private readonly IRepository<SafeProblemRecord, Guid> _safeProblemRecord;
        private readonly IRepository<SafeProblemRltCcUser, Guid> _safeProblemRltCcUsers;
        private readonly IRepository<SafeProblemRltEquipment, Guid> _safeProblemRltEquipment;
        private readonly IRepository<SafeProblemRltFile, Guid> _safeProblemRltFiles;
        protected IIdentityUserRepository _userRepository { get; }
        private readonly IMessageNoticeProvider _messageNotice;
        private readonly IGuidGenerator _guidGenerator;
        public SafeProblemAppService(
             IRepository<SafeProblem, Guid> safeProblems,
             IRepository<SafeProblemLibrary, Guid> safeProblemLibrary,
             IRepository<SafeProblemRecord, Guid> safeProblemRecord,
             IMessageNoticeProvider messageNotice,
              IIdentityUserRepository userRepository,
              IRepository<SafeProblemRltCcUser, Guid> safeProblemRltCcUsers,
              IRepository<SafeProblemRltEquipment, Guid> safeProblemRltEquipment,
              IRepository<SafeProblemRltFile, Guid> safeProblemRltFiles,
              IGuidGenerator guidGenerator
            )
        {
            _guidGenerator = guidGenerator;
            _safeProblemLibrary = safeProblemLibrary;
            _userRepository = userRepository;
            _safeProblems = safeProblems;
            _safeProblemRecord = safeProblemRecord;
            _messageNotice = messageNotice;
            _safeProblemRltCcUsers = safeProblemRltCcUsers;
            _safeProblemRltEquipment = safeProblemRltEquipment;
            _safeProblemRltFiles = safeProblemRltFiles;
        }

        public Task<SafeProblemDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");

            var safeProblem = _safeProblems.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (safeProblem == null) throw new UserFriendlyException("当前数据不存在");

            return Task.FromResult(ObjectMapper.Map<SafeProblem, SafeProblemDto>(safeProblem));
        }

        public Task<PagedResultDto<SafeProblemDto>> GetList(SafeProblemSearchDto input)
        {
            var safeProblems = _safeProblems.WithDetails()
                    .WhereIf(input.IsSelect, x => x.State == SafeProblemState.Improved)
                   .WhereIf(input.FilterType == SafeFilterType.MyChecked, x => x.CheckerId == CurrentUser.Id)
                   .WhereIf(input.FilterType == SafeFilterType.MyWaitingImprove, x => x.ResponsibleUserId == CurrentUser.Id && x.State == SafeProblemState.WaitingImprove)
                   .WhereIf(input.FilterType == SafeFilterType.MyWaitingVerify, x => x.VerifierId == CurrentUser.Id && x.State == SafeProblemState.WaitingVerifie)
                   .WhereIf(input.FilterType == SafeFilterType.CopyMine, x => x.CcUsers.Any(y => y.CcUserId == CurrentUser.Id))
                   .WhereIf(!string.IsNullOrEmpty(input.Title), x => x.Title.Contains(input.Title))
                   .WhereIf(input.RiskLevel != null, x => x.RiskLevel == input.RiskLevel)
                   .WhereIf(input.TypeId != null && input.TypeId != Guid.Empty, x => x.TypeId == input.TypeId)
                   .WhereIf(input.StartTime != null && input.EndTime != null, x => x.CheckTime >= input.StartTime && x.CheckTime <= input.EndTime);


            var result = new PagedResultDto<SafeProblemDto>();
            safeProblems = safeProblems.OrderBy(x => x.State);
            result.TotalCount = safeProblems.Count();
            if (input.IsAll)
            {
                result.Items = ObjectMapper.Map<List<SafeProblem>, List<SafeProblemDto>>(safeProblems.OrderBy(x => x.State).ThenByDescending(x => x.LimitTime).ToList());

            }
            else
            {
                result.Items = ObjectMapper.Map<List<SafeProblem>, List<SafeProblemDto>>(safeProblems.OrderBy(x => x.State).ThenByDescending(x => x.LimitTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            foreach (var item in result.Items)
            {
                var ChangeTime = _safeProblemRecord.Where(x => x.SafeProblemId == item.Id).OrderBy(x => x.CreationTime).ToList();
                if (ChangeTime.Count > 0) { item.ChangeTime = ChangeTime.LastOrDefault().Time; }

            }

            return Task.FromResult(result);
        }

        /// <summary>
        /// 列表待整改问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<SafeProblemDto>> GetWaitingImproveList(SafeProblemSearchDto input)
        {
            var result = new PagedResultDto<SafeProblemDto>();
            var safeProblems = _safeProblems.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Title), x => x.Title.Contains(input.Title))
                .Where(x => x.State == SafeProblemState.WaitingImprove).ToList();
            if (input.IsAll)
            {
                result.Items = ObjectMapper.Map<List<SafeProblem>, List<SafeProblemDto>>(safeProblems.OrderBy(x => x.State).ThenByDescending(x => x.LimitTime).ToList());

            }
            else
            {
                result.Items = ObjectMapper.Map<List<SafeProblem>, List<SafeProblemDto>>(safeProblems.OrderBy(x => x.State).ThenByDescending(x => x.LimitTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            result.TotalCount = safeProblems.Count();
            return Task.FromResult(result);
        }
        public Task<List<SafeProblemRecordDto>> GetRecordList(Guid? id)
        {
            var safeProblemRecords = _safeProblemRecord.WithDetails().Where(x => x.SafeProblemId == id).ToList();
            return Task.FromResult(ObjectMapper.Map<List<SafeProblemRecord>, List<SafeProblemRecordDto>>(safeProblemRecords));
        }

        public async Task<SafeProblemDto> Create(SafeProblemCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Title.Trim())) throw new UserFriendlyException("标题不能为空");
            if (input.TypeId == null || input.TypeId == Guid.Empty) throw new UserFriendlyException("问题类型不能为空");
            if (input.ProfessionId == null || input.ProfessionId == Guid.Empty) throw new UserFriendlyException("所属专业不能为空");
            if (input.CheckTime == null) throw new UserFriendlyException("检查时间不能为空");
            if (input.LimitTime == null) throw new UserFriendlyException("限期时间不能为空");
            if (input.CheckerId == null || input.CheckerId == Guid.Empty) throw new UserFriendlyException("检查人不能为空");
            if (input.ResponsibleUserId == null || input.ResponsibleUserId == Guid.Empty) throw new UserFriendlyException("责任人不能为空");
            if (input.CcUsers.Count == 0) throw new UserFriendlyException("抄送人不能为空");
            if (string.IsNullOrEmpty(input.ResponsibleUnit)) throw new UserFriendlyException("责任单位不能为空");
            if (_safeProblems.FirstOrDefault(x => x.Title == input.Title) != null) throw new UserFriendlyException("该问题标题已存在");
            var date = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");
            var code = "SF" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace("-", "");

            var safeProblem = new SafeProblem(_guidGenerator.Create())
            {
                Code = code,
                Title = input.Title,
                TypeId = input.TypeId,
                ProfessionId = input.ProfessionId,
                RiskLevel = input.RiskLevel,
                CheckTime = input.CheckTime,
                CheckUnitId = input.CheckUnitId,
                CheckUnitName = input.CheckUnitName,
                LimitTime = input.LimitTime,
                CheckerId = input.CheckerId,
                ResponsibleUserId = input.ResponsibleUserId,
                VerifierId = input.VerifierId,
                ResponsibleUnit = input.ResponsibleUnit,
                ResponsibleOrganizationId = input.ResponsibleOrganizationId,
                Content = input.Content,
                Suggestion = input.Suggestion,
                State = SafeProblemState.WaitingImprove
            };
            safeProblem.CcUsers = new List<SafeProblemRltCcUser>();
            // 保存抄送人信息
            foreach (var user in input.CcUsers)
            {
                safeProblem.CcUsers.Add(new SafeProblemRltCcUser(_guidGenerator.Create())
                {
                    SafeProblemId = safeProblem.Id,
                    CcUserId = user.CcUserId
                });
            }

            safeProblem.Files = new List<SafeProblemRltFile>();
            // 保存附件信息
            foreach (var file in input.Files)
            {
                safeProblem.Files.Add(new SafeProblemRltFile(_guidGenerator.Create())
                {
                    SafeProblemId = safeProblem.Id,
                    FileId = file.FileId
                });
            }

            safeProblem.Equipments = new List<SafeProblemRltEquipment>();
            // 保存模型信息
            foreach (var equipment in input.Equipments)
            {
                safeProblem.Equipments.Add(new SafeProblemRltEquipment(_guidGenerator.Create())
                {
                    SafeProblemId = safeProblem.Id,
                    EquipmentId = equipment.EquipmentId
                });
            }

            await _safeProblems.InsertAsync(safeProblem);

            //if (safeProblem.RiskLevel == SafetyRiskLevel.Especially || safeProblem.RiskLevel == SafetyRiskLevel.Great)
            //{
            //    var problemLibrary = new SafeProblemLibrary(_guidGenerator.Create())
            //    {
            //        Title = safeProblem.Title,
            //        Measures = safeProblem.Suggestion,
            //        Content = safeProblem.Content,
            //        RiskLevel = safeProblem.RiskLevel,
            //        ProfessionId = safeProblem.ProfessionId,
            //    };
            //    await _safeProblemLibrary.InsertAsync(problemLibrary);
            //}

            //await SendMessageAsync(safeProblem.ResponsibleUserId, SafeMessageType.ImproveMessage, "SafeProblem", null);

            return ObjectMapper.Map<SafeProblem, SafeProblemDto>(safeProblem);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SafeProblemDto> Update(SafeProblemUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Title.Trim())) throw new UserFriendlyException("问题标题不能为空");
            if (input.TypeId == null || input.TypeId == Guid.Empty) throw new UserFriendlyException("问题类型不能为空");
            if (input.CheckTime == null) throw new UserFriendlyException("检查时间不能为空");
            if (input.LimitTime == null) throw new UserFriendlyException("限期时间不能为空");
            if (input.CheckerId == null || input.CheckerId == Guid.Empty) throw new UserFriendlyException("检查人不能为空");
            if (input.ResponsibleUserId == null || input.ResponsibleUserId == Guid.Empty) throw new UserFriendlyException("责任人不能为空");
            if (input.CcUsers.Count == 0) throw new UserFriendlyException("抄送人不能为空");
            if (string.IsNullOrEmpty(input.ResponsibleUnit)) throw new UserFriendlyException("责任单位不能为空");
            if (_safeProblems.FirstOrDefault(x => x.Title == input.Title && x.Id != input.Id) != null) throw new UserFriendlyException("该问题标题已存在");

            var safeProblem = _safeProblems.FirstOrDefault(x => x.Id == input.Id);
            if (safeProblem == null) throw new UserFriendlyException("更新实体不存在");
            safeProblem.Code = safeProblem.Code;
            safeProblem.Title = input.Title;
            safeProblem.CheckTime = input.CheckTime;
            safeProblem.CheckerId = input.CheckerId;
            safeProblem.CheckUnitId = input.CheckUnitId;
            safeProblem.CheckUnitName = input.CheckUnitName;
            safeProblem.LimitTime = input.LimitTime;
            safeProblem.TypeId = input.TypeId;
            safeProblem.ResponsibleUnit = input.ResponsibleUnit;
            safeProblem.ResponsibleUserId = input.ResponsibleUserId;
            safeProblem.ResponsibleOrganizationId = input.ResponsibleOrganizationId;
            safeProblem.Content = input.Content;
            safeProblem.Suggestion = input.Suggestion;
            safeProblem.VerifierId = input.VerifierId;
            safeProblem.State = SafeProblemState.WaitingImprove;
            // 清楚之前关联信息
            await _safeProblemRltCcUsers.DeleteAsync(x => x.SafeProblemId == safeProblem.Id);
            safeProblem.CcUsers = new List<SafeProblemRltCcUser>();
            // 保存抄送人信息
            foreach (var user in input.CcUsers)
            {
                safeProblem.CcUsers.Add(new SafeProblemRltCcUser(_guidGenerator.Create())
                {
                    SafeProblemId = safeProblem.Id,
                    CcUserId = user.CcUserId
                });
            }

            // 清楚之前关联信息
            await _safeProblemRltFiles.DeleteAsync(x => x.SafeProblemId == safeProblem.Id);
            safeProblem.Files = new List<SafeProblemRltFile>();
            // 保存附件信息
            foreach (var file in input.Files)
            {
                safeProblem.Files.Add(new SafeProblemRltFile(_guidGenerator.Create())
                {
                    SafeProblemId = safeProblem.Id,
                    FileId = file.FileId
                });
            }

            // 清楚之前关联信息
            await _safeProblemRltEquipment.DeleteAsync(x => x.SafeProblemId == safeProblem.Id);
            safeProblem.Equipments = new List<SafeProblemRltEquipment>();
            // 保存模型信息
            foreach (var equipment in input.Equipments)
            {
                safeProblem.Equipments.Add(new SafeProblemRltEquipment(_guidGenerator.Create())
                {
                    SafeProblemId = safeProblem.Id,
                    EquipmentId = equipment.EquipmentId
                });
            }
            await _safeProblems.UpdateAsync(safeProblem);

            //if (safeProblem.RiskLevel == SafetyRiskLevel.Especially || safeProblem.RiskLevel == SafetyRiskLevel.Great)
            //{
            //    if (_safeProblemLibrary.FirstOrDefault(x => x.Title == safeProblem.Title &&
            //                                              x.Measures == safeProblem.Suggestion &&
            //                                              x.Content == safeProblem.Content &&
            //                                              x.RiskLevel == safeProblem.RiskLevel &&
            //                                              x.ProfessionId == safeProblem.ProfessionId) == null)
            //    {
            //        var problemLibrary = new SafeProblemLibrary(_guidGenerator.Create())
            //        {
            //            Title = safeProblem.Title,
            //            Measures = safeProblem.Suggestion,
            //            Content = safeProblem.Content,
            //            RiskLevel = safeProblem.RiskLevel,
            //            ProfessionId = safeProblem.ProfessionId,
            //        };
            //        await _safeProblemLibrary.InsertAsync(problemLibrary);
            //    }

            //}

            //await SendMessageAsync(safeProblem.ResponsibleUserId, SafeMessageType.ImproveMessage, "SafeProblem", null);
            return ObjectMapper.Map<SafeProblem, SafeProblemDto>(safeProblem);
        }
        public async Task<SafeProblemRecordDto> CreateRecord(SafeProblemRecordCreateDto input)
        {
            if (input.SafeProblemId == null || input.SafeProblemId == Guid.Empty) throw new UserFriendlyException("请选择您要操作的数据");
            var safeProblemRecord = ObjectMapper.Map<SafeProblemRecordCreateDto, SafeProblemRecord>(input);
            safeProblemRecord.SetId(_guidGenerator.Create());

            safeProblemRecord.Files = new List<SafeProblemRecordRltFile>();
            // 保存附件信息
            foreach (var file in input.Files)
            {
                safeProblemRecord.Files.Add(new SafeProblemRecordRltFile(_guidGenerator.Create())
                {
                    FileId = file.FileId
                });
            }
            var safeProblem = _safeProblems.WithDetails().Where(x => x.Id == input.SafeProblemId).FirstOrDefault();
            if (safeProblem == null) throw new UserFriendlyException("您要操作的问题数据不存在,请刷新页面重新尝试");
            /*如果是整改*/
            if (safeProblem.State.ToString() == SafeProblemState.WaitingImprove.ToString())
            {
                safeProblem.State = SafeProblemState.WaitingVerifie;
                //await SendMessageAsync(safeProblem.VerifierId, SafeMessageType.VerifyMessage, "SafeProblemRecord", SafeRecordType.Improve);
            }

            /*如果是验证，不通过*/
            else if (safeProblem.State.ToString() == SafeProblemState.WaitingVerifie.ToString() && safeProblemRecord.State == SafeRecordState.NotPass)
            {
                safeProblem.State = SafeProblemState.WaitingImprove;
                //await SendMessageAsync(safeProblem.ResponsibleUserId, SafeMessageType.ImproveMessageNotPass, "SafeProblemRecord", SafeRecordType.Verify);
            }
            /*如果是验证，通过*/
            else if (safeProblem.State.ToString() == SafeProblemState.WaitingVerifie.ToString() && safeProblemRecord.State == SafeRecordState.Passed)
            {
                safeProblem.State = SafeProblemState.Improved;
                //await SendMessageAsync(safeProblem.ResponsibleUserId, SafeMessageType.ImproveMessagePassed, "SafeProblemRecord", SafeRecordType.Verify);
            }
            await _safeProblemRecord.InsertAsync(safeProblemRecord);

            await _safeProblems.UpdateAsync(safeProblem);
            return ObjectMapper.Map<SafeProblemRecord, SafeProblemRecordDto>(safeProblemRecord);
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            await _safeProblems.DeleteAsync(id);

            return true;
        }

        public async Task<Stream> Export(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var problem = _safeProblems.WithDetails().FirstOrDefault(x => x.Id == id);
            if (problem == null) throw new UserFriendlyException("该安全问题不存在");

            var problemRecords = _safeProblemRecord.Where(x => x.SafeProblemId == id).ToList();

            string fileName = "安全问题报告明细";

            string tableName = "整改验证记录";

            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();

            //文本标题
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, fileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 10, "宋体", ParagraphAlignment.CENTER), 1);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 10, "宋体", ParagraphAlignment.CENTER), 2);

            //TODO:这里一行需要显示两个文本
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"问题标题：{problem.Title}", false, 10, "宋体", ParagraphAlignment.LEFT, true, $"       问题类型：{problem.Type?.Name}"), 3);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"检查时间：{problem.CheckTime.ToString()}", false, 10, "宋体", ParagraphAlignment.LEFT, true, $"       限期时间：{problem.LimitTime.ToString()}"), 4);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"检查人：{problem.Checker?.Name}", false, 10, "宋体", ParagraphAlignment.LEFT, true, $"       责任人：{problem.ResponsibleUser?.Name}"), 5);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"抄送人：{problem.CcUsers.Select(x => x.CcUser?.Name).JoinAsString("/")}", false, 10, "宋体", ParagraphAlignment.LEFT, true, $"       审核人：{problem.Verifier?.Name}"), 6);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"责任单位：{problem.ResponsibleUnit}", false, 10, "宋体", ParagraphAlignment.LEFT, true, $"       责任部门：{problem.ResponsibleOrganization?.Name}"), 7);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"问题描述：{problem.Content}", false, 10, "宋体", ParagraphAlignment.LEFT, true), 8);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"整改意见：{problem.Suggestion}", false, 10, "宋体", ParagraphAlignment.LEFT, true), 9);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", false, 10, "宋体", ParagraphAlignment.LEFT, true), 10);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"{tableName}", false, 10, "宋体", ParagraphAlignment.CENTER, true), 11);

            #region 文档第表格对象实例（遍历表格项）
            //创建文档中的表格对象实例
            XWPFTable xwpfTable = document.CreateTable(problemRecords.Count + 1, 4);//显示的行列数rows:8行,cols:7列
            xwpfTable.Width = 5200;//总宽度
            xwpfTable.SetColumnWidth(0, 500); /* 设置列宽 */
            xwpfTable.SetColumnWidth(1, 500); /* 设置列宽 */
            xwpfTable.SetColumnWidth(2, 500); /* 设置列宽 */
            xwpfTable.SetColumnWidth(3, 3700); /* 设置列宽 */

            //遍历表格标题
            xwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "序号", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "记录类型", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "操作人", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "整改内容", ParagraphAlignment.CENTER, 22, true));
            var i = 1;
            //遍历数据
            foreach (var item in problemRecords)
            {
                xwpfTable.GetRow(i).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{i}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{(item.Type == SafeRecordType.Improve ? "整改" : "验证")}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.User?.Name}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Content}", ParagraphAlignment.CENTER, 22, false));
                i++;
            }

            #endregion
            //向文档流中写入内容，生成word
            MemoryStream stream = new MemoryStream();
            document.Write(stream);
            var buf = stream.ToArray();
            stream = new MemoryStream(buf);
            return stream;
        }

        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="MType">消息问题类型</param>
        /// <param name="Type">模块类型</param>
        /// <param name="RType">内容类型</param>
        /// <returns></returns>
        private async Task SendMessageAsync(Guid? Id, SafeMessageType MType, string Type, SafeRecordType? RType)
        {
            ///保存成功后给通知人员发送消息
            var message = new NoticeMessage();
            message.SendType = Message.MessageDefine.SendModeType.User;

            // 获取通知人员的ids
            message.SetUserId(Id.Value);
            var content = new NoticeMessageContent();
            content.Type = Type;
            content.SponsorId = CurrentUser.Id.Value;
            var safeMessageNoticeDto = new SafeMessageNoticeDto();
            //获取提交消息的人
            var user = await _userRepository.GetAsync(CurrentUser.Id.Value);
            safeMessageNoticeDto.Name = user.Name;
            safeMessageNoticeDto.RecordType = RType.ToString() == SafeRecordType.Improve.ToString() ? "整改" : RType.ToString() == SafeRecordType.Verify.ToString() ? "验证" : "创建";
            var info = "";
            if (MType == SafeMessageType.ImproveMessage)
            {
                info = "您有待整改的安全问题！";
            }
            else if (MType == SafeMessageType.VerifyMessage)
            {
                info = "您有待验证的安全问题！";
            }
            else if (MType == SafeMessageType.ImproveMessageNotPass)
            {
                info = "您提交的安全问题整改未通过！";
            }
            else if (MType == SafeMessageType.ImproveMessagePassed)
            {
                info = "您提交的安全问题整改已通过！";
            }
            safeMessageNoticeDto.RecordContent = info;
            content.CreateContent(safeMessageNoticeDto);
            message.SetContent(content);
            await _messageNotice.PushAsync(message.GetBinary());
        }

        public Task<SafeProblemReportDto> GetSafeProblemReport(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var safeProblem = _safeProblems.WithDetails().Where(x => x.Id == id).FirstOrDefault();

            if (safeProblem == null) throw new UserFriendlyException("当前数据不存在");
            var safeProblemDto = ObjectMapper.Map<SafeProblem, SafeProblemReportDto>(safeProblem);
            var safeProblemRecords = _safeProblemRecord.WithDetails().Where(x => x.SafeProblemId == safeProblem.Id).ToList();
            safeProblemDto.SafeProblemRecord = safeProblemRecords;
            foreach (var item in safeProblemRecords)
            {
                if (item.Type == SafeRecordType.Improve)
                {
                    safeProblemDto.ImproveRecord.Add(item);
                }
                if (item.Type == SafeRecordType.Verify)
                {
                    safeProblemDto.VerifyRecord.Add(item);
                }
            }
            return Task.FromResult(safeProblemDto);
        }
    }
}
