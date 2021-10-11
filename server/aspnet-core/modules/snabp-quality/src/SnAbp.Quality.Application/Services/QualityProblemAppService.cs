using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.XWPF.UserModel;
using SnAbp.Message.Notice;
using SnAbp.Quality.Dtos;
using SnAbp.Quality.Entities;
using SnAbp.Quality.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Quality.Services
{
    public class QualityProblemAppService : QualityAppService, IQualityProblemAppService
    {
        private readonly IRepository<QualityProblem, Guid> _problemsRepository;
        private readonly IRepository<QualityProblemLibrary, Guid> _problemLibraryRepository;
        private readonly IRepository<QualityProblemRecord, Guid> _recordRepository;
        private readonly IMessageNoticeProvider _messageNotice;
        private readonly IRepository<QualityProblemRecordRltFile, Guid> _recordRltFileRepository;
        private readonly IRepository<QualityProblemRltCcUser, Guid> _problemRltCcUsersRepository;
        private readonly IRepository<QualityProblemRltEquipment, Guid> _problemRltEquipmentRepository;
        private readonly IRepository<QualityProblemRltFile, Guid> _problemRltFilesRepository;
        private readonly IGuidGenerator _generator;
        public QualityProblemAppService(
             IRepository<QualityProblem, Guid> problemsRepository,
             IRepository<QualityProblemLibrary, Guid> problemLibraryRepository,
             IRepository<QualityProblemRecord, Guid> recordRepository,
             IMessageNoticeProvider messageNotice,
             IRepository<QualityProblemRecordRltFile, Guid> recordRltFileRepository,
             IRepository<QualityProblemRltCcUser, Guid> problemRltCcUsersRepository,
             IRepository<QualityProblemRltEquipment, Guid> problemRltEquipmentRepository,
             IRepository<QualityProblemRltFile, Guid> problemRltFilesRepository,
             IGuidGenerator generator

            )
        {
            _problemsRepository = problemsRepository;
            _problemLibraryRepository = problemLibraryRepository;
            _recordRepository = recordRepository;
            _messageNotice = messageNotice;
            _recordRltFileRepository = recordRltFileRepository;
            _problemRltCcUsersRepository = problemRltCcUsersRepository;
            _problemRltEquipmentRepository = problemRltEquipmentRepository;
            _problemRltFilesRepository = problemRltFilesRepository;
            _generator = generator;
        }
        /// <summary>
        /// 详情获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<QualityProblemDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var qualityProblem = _problemsRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (qualityProblem == null)
            {
                throw new UserFriendlyException("该质量问题不存在");
            }

            return Task.FromResult(ObjectMapper.Map<QualityProblem, QualityProblemDto>(qualityProblem));
        }

        /// <summary>
        /// 列表获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<QualityProblemDto>> GetList(QualityProblemSearchDto input)
        {
            var qualityProblems = _problemsRepository.WithDetails()
                .WhereIf(input.FilterType == QualityFilterType.MyChecked, x => x.CheckerId == CurrentUser.Id)
                .WhereIf(input.IsSelect, x => x.State == QualityProblemState.Improved)
                   .WhereIf(input.FilterType == QualityFilterType.MyWaitingImprove, x => x.ResponsibleUserId == CurrentUser.Id && x.State == QualityProblemState.WaitingImprove)
                   .WhereIf(input.FilterType == QualityFilterType.MyWaitingVerify, x => x.VerifierId == CurrentUser.Id && x.State == QualityProblemState.WaitingVerifie)
                   .WhereIf(input.FilterType == QualityFilterType.CopyMine, x => x.CcUsers.Any(y => y.CcUserId == CurrentUser.Id))
                   .WhereIf(!string.IsNullOrEmpty(input.Title), x => x.Title.Contains(input.Title))
                   .WhereIf(input.Type != null, x => x.Type == input.Type)
                   .WhereIf(input.StartTime != null && input.EndTime != null, x => x.CheckTime >= input.StartTime && x.CheckTime <= input.EndTime).OrderBy(x => x.CreationTime);

            var result = new PagedResultDto<QualityProblemDto>();
            qualityProblems = qualityProblems.OrderBy(x => x.State);
            result.TotalCount = qualityProblems.Count();
            if (input.IsAll)
            {
                result.Items = ObjectMapper.Map<List<QualityProblem>, List<QualityProblemDto>>(qualityProblems.OrderBy(x => x.State).ThenByDescending(x => x.LimitTime).ToList());

            }
            else
            {
                result.Items = ObjectMapper.Map<List<QualityProblem>, List<QualityProblemDto>>(qualityProblems.OrderBy(x => x.State).ThenByDescending(x => x.LimitTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            foreach (var item in result.Items)
            {
                var ChangeTime = _recordRepository.Where(x => x.QualityProblemId == item.Id).OrderBy(x => x.CreationTime).ToList();
                if (ChangeTime.Count > 0) { item.ChangeTime = ChangeTime.LastOrDefault().Time; }

            }

            return Task.FromResult(result);

        }

        /// <summary>
        /// 列表待整改问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<QualityProblemDto>> GetWaitingImproveList(QualityProblemSearchDto input)
        {
            var result = new PagedResultDto<QualityProblemDto>();
            var qualityProblems = _problemsRepository.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Title), x => x.Title.Contains(input.Title))
                .Where(x => x.State == QualityProblemState.WaitingImprove).ToList();
            if (input.IsAll)
            {
                result.Items = ObjectMapper.Map<List<QualityProblem>, List<QualityProblemDto>>(qualityProblems.OrderBy(x => x.State).ThenByDescending(x => x.LimitTime).ToList());

            }
            else
            {
                result.Items = ObjectMapper.Map<List<QualityProblem>, List<QualityProblemDto>>(qualityProblems.OrderBy(x => x.State).ThenByDescending(x => x.LimitTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            result.TotalCount = qualityProblems.Count();
            return Task.FromResult(result);
        }

        /// <summary>
        /// 质量报告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<QualityProblemReportDto> GetQualityProblemReport(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var qualityProblem = _problemsRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();

            if (qualityProblem == null) throw new UserFriendlyException("当前数据不存在");
            var qualityProblemDto = ObjectMapper.Map<QualityProblem, QualityProblemReportDto>(qualityProblem);
            var qualityProblemRecords = _recordRepository.WithDetails().Where(x => x.QualityProblemId == qualityProblem.Id).ToList();
            qualityProblemDto.QualityProblemRecord = qualityProblemRecords;
            foreach (var item in qualityProblemRecords)
            {
                if (item.Type == QualityRecordType.Improve)
                {
                    qualityProblemDto.ImproveRecord.Add(item);
                }
                if (item.Type == QualityRecordType.Verify)
                {
                    qualityProblemDto.VerifyRecord.Add(item);
                }
            }
            return Task.FromResult(qualityProblemDto);
        }
        /// <summary>
        /// 记录列表获取
        /// </summary>
        /// <returns></returns>
        public Task<List<QualityProblemRecordDto>> GetRecordList(Guid id)
        {
            var qualityProblemRecords = _recordRepository.WithDetails().Where(x => x.QualityProblemId == id).ToList();
            return Task.FromResult(ObjectMapper.Map<List<QualityProblemRecord>, List<QualityProblemRecordDto>>(qualityProblemRecords));
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<QualityProblemDto> Create(QualityProblemCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Title.Trim())) throw new UserFriendlyException("问题标题不能为空");
            if (input.CheckTime == null) throw new UserFriendlyException("检查时间不能为空");
            if (input.LimitTime == null) throw new UserFriendlyException("限期时间不能为空");
            if (input.CheckerId == null || input.CheckerId == Guid.Empty) throw new UserFriendlyException("检查人不能为空");
            if (input.ResponsibleUserId == null || input.ResponsibleUserId == Guid.Empty) throw new UserFriendlyException("责任人不能为空");
            if (input.CcUsers.Count == 0) throw new UserFriendlyException("抄送人不能为空");
            if (string.IsNullOrEmpty(input.ResponsibleUnit)) throw new UserFriendlyException("责任单位不能为空");
            if (_problemsRepository.FirstOrDefault(x => x.Title == input.Title) != null) throw new UserFriendlyException("该问题标题已存在");

            var date = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");

            var code = "QL" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace("-", "");

            var qualityProblem = new QualityProblem(_generator.Create())
            {
                Code = code,
                Title = input.Title,
                CheckTime = input.CheckTime,
                CheckerId = input.CheckerId,
                CheckUnitId = input.CheckUnitId,
                CheckUnitName = input.CheckUnitName,
                ProfessionId = input.ProfessionId,
                LimitTime = input.LimitTime,
                Type = input.Type,
                Level = input.Level,
                ResponsibleUnit = input.ResponsibleUnit,
                ResponsibleUserId = input.ResponsibleUserId,
                ResponsibleOrganizationId = input.ResponsibleOrganizationId,
                Content = input.Content,
                Suggestion = input.Suggestion,
                VerifierId = input.VerifierId,
                State = QualityProblemState.WaitingImprove
            };

            qualityProblem.CcUsers = new List<QualityProblemRltCcUser>();
            // 保存抄送人信息
            foreach (var user in input.CcUsers)
            {
                qualityProblem.CcUsers.Add(new QualityProblemRltCcUser(_generator.Create())
                {
                    QualityProblemId = qualityProblem.Id,
                    CcUserId = user.CcUserId
                });
            }

            qualityProblem.Files = new List<QualityProblemRltFile>();
            // 保存附件信息
            foreach (var file in input.Files)
            {
                qualityProblem.Files.Add(new QualityProblemRltFile(_generator.Create())
                {
                    QualityProblemId = qualityProblem.Id,
                    FileId = file.FileId
                });
            }

            qualityProblem.Equipments = new List<QualityProblemRltEquipment>();
            // 保存模型信息
            foreach (var equipment in input.Equipments)
            {
                qualityProblem.Equipments.Add(new QualityProblemRltEquipment(_generator.Create())
                {
                    QualityProblemId = qualityProblem.Id,
                    EquipmentId = equipment.EquipmentId
                });
            }

            await _problemsRepository.InsertAsync(qualityProblem);

            //if (qualityProblem.Type == QualityProblemType.A)
            //{
            //    var problemLibrary = new QualityProblemLibrary(_generator.Create())
            //    {
            //        Title = qualityProblem.Title,
            //        Measures = qualityProblem.Suggestion,
            //        Content = qualityProblem.Content,
            //        ProfessionId = qualityProblem.ProfessionId,
            //        Type = qualityProblem.Type,
            //        Level = qualityProblem.Level,
            //    };
            //    await _problemLibraryRepository.InsertAsync(problemLibrary);
            //}

            //await SendMessageAsync(qualityProblem.ResponsibleUserId, QualityMessageType.ImproveMessage, qualityProblem.CreatorId);

            return ObjectMapper.Map<QualityProblem, QualityProblemDto>(qualityProblem);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QualityProblemDto> Update(QualityProblemUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Title.Trim())) throw new UserFriendlyException("问题标题不能为空");
            if (input.CheckTime == null) throw new UserFriendlyException("检查时间不能为空");
            if (input.LimitTime == null) throw new UserFriendlyException("限期时间不能为空");
            if (input.CheckerId == null || input.CheckerId == Guid.Empty) throw new UserFriendlyException("检查人不能为空");
            if (input.ResponsibleUserId == null || input.ResponsibleUserId == Guid.Empty) throw new UserFriendlyException("责任人不能为空");
            if (input.CcUsers.Count == 0) throw new UserFriendlyException("抄送人不能为空");
            if (string.IsNullOrEmpty(input.ResponsibleUnit)) throw new UserFriendlyException("责任单位不能为空");
            if (_problemsRepository.FirstOrDefault(x => x.Title == input.Title && x.Id != input.Id) != null) throw new UserFriendlyException("该问题标题已存在");

            var qualityProblem = _problemsRepository.FirstOrDefault(x => x.Id == input.Id);
            if (qualityProblem == null) throw new UserFriendlyException("更新实体不存在");

            qualityProblem.Code = qualityProblem.Code;
            qualityProblem.Title = input.Title;
            qualityProblem.CheckTime = input.CheckTime;
            qualityProblem.CheckerId = input.CheckerId;
            qualityProblem.LimitTime = input.LimitTime;
            qualityProblem.CheckUnitId = input.CheckUnitId;
            qualityProblem.CheckUnitName = input.CheckUnitName;
            qualityProblem.Type = input.Type;
            qualityProblem.Level = input.Level;
            qualityProblem.ProfessionId = input.ProfessionId;
            qualityProblem.ResponsibleUnit = input.ResponsibleUnit;
            qualityProblem.ResponsibleUserId = input.ResponsibleUserId;
            qualityProblem.ResponsibleOrganizationId = input.ResponsibleOrganizationId;
            qualityProblem.Content = input.Content;
            qualityProblem.Suggestion = input.Suggestion;
            qualityProblem.VerifierId = input.VerifierId;
            qualityProblem.State = QualityProblemState.WaitingImprove;

            // 清楚之前关联信息
            await _problemRltCcUsersRepository.DeleteAsync(x => x.QualityProblemId == qualityProblem.Id);
            qualityProblem.CcUsers = new List<QualityProblemRltCcUser>();
            // 保存抄送人信息
            foreach (var user in input.CcUsers)
            {
                qualityProblem.CcUsers.Add(new QualityProblemRltCcUser(_generator.Create())
                {
                    QualityProblemId = qualityProblem.Id,
                    CcUserId = user.CcUserId
                });
            }

            // 清楚之前关联信息
            await _problemRltFilesRepository.DeleteAsync(x => x.QualityProblemId == qualityProblem.Id);
            qualityProblem.Files = new List<QualityProblemRltFile>();
            // 保存附件信息
            foreach (var file in input.Files)
            {
                qualityProblem.Files.Add(new QualityProblemRltFile(_generator.Create())
                {
                    QualityProblemId = qualityProblem.Id,
                    FileId = file.FileId
                });
            }

            // 清楚之前关联信息
            await _problemRltEquipmentRepository.DeleteAsync(x => x.QualityProblemId == qualityProblem.Id);
            qualityProblem.Equipments = new List<QualityProblemRltEquipment>();
            // 保存模型信息
            foreach (var equipment in input.Equipments)
            {
                qualityProblem.Equipments.Add(new QualityProblemRltEquipment(_generator.Create())
                {
                    QualityProblemId = qualityProblem.Id,
                    EquipmentId = equipment.EquipmentId
                });
            }

            await _problemsRepository.UpdateAsync(qualityProblem);
            //if (qualityProblem.Type == QualityProblemType.A)
            //{
            //    if (_problemLibraryRepository.FirstOrDefault(x => x.Title == qualityProblem.Title &&
            //                                                                     x.Measures == qualityProblem.Suggestion &&
            //                                                                     x.Content == qualityProblem.Content &&
            //                                                                     x.ProfessionId == qualityProblem.ProfessionId &&
            //                                                                     x.Type == qualityProblem.Type &&
            //                                                                     x.Level == qualityProblem.Level) == null)
            //    {
            //        var problemLibrary = new QualityProblemLibrary(_generator.Create())
            //        {
            //            Title = qualityProblem.Title,
            //            Measures = qualityProblem.Suggestion,
            //            Content = qualityProblem.Content,
            //            ProfessionId = qualityProblem.ProfessionId,
            //            Type = qualityProblem.Type,
            //            Level = qualityProblem.Level,
            //        };
            //        await _problemLibraryRepository.InsertAsync(problemLibrary);
            //    }
            //}

            //await SendMessageAsync(qualityProblem.ResponsibleUserId, QualityMessageType.ImproveMessage, qualityProblem.CreatorId);

            return ObjectMapper.Map<QualityProblem, QualityProblemDto>(qualityProblem);
        }

        /// <summary>
        /// 记录创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<QualityProblemRecordDto> CreateRecord(QualityProblemRecordCreateDto input)
        {
            if (input.QualityProblemId == null || input.QualityProblemId == Guid.Empty) throw new UserFriendlyException("请选择要整改的质量问题");
            if (input.Time == null) throw new UserFriendlyException("时间不能为空");
            if (input.UserId == null || input.UserId == Guid.Empty) throw new UserFriendlyException("操作人员不能为空");

            var qualityProblem = _problemsRepository.FirstOrDefault(x => x.Id == input.QualityProblemId);
            if (qualityProblem == null) throw new UserFriendlyException("整改问题不存在");

            var qualityProblemRecord = new QualityProblemRecord(_generator.Create())
            {
                QualityProblemId = input.QualityProblemId,
                UserId = input.UserId,
                Type = input.Type,
                Content = input.Content,
                State = input.State,
                Time = input.Time,
            };

            qualityProblemRecord.Files = new List<QualityProblemRecordRltFile>();
            // 保存附件信息
            foreach (var file in input.Files)
            {
                qualityProblemRecord.Files.Add(new QualityProblemRecordRltFile(_generator.Create())
                {
                    QualityProblemRecordId = qualityProblemRecord.Id,
                    FileId = file.FileId
                });
            }

            if (qualityProblemRecord.State == QualityRecordState.Checking)
            {
                qualityProblem.State = QualityProblemState.WaitingVerifie;
                //await SendMessageAsync(qualityProblemRecord.UserId, QualityMessageType.VerifyMessage, qualityProblemRecord.CreatorId);
            }
            else if (qualityProblemRecord.State == QualityRecordState.NotPass)
            {
                qualityProblem.State = QualityProblemState.WaitingImprove;
                //await SendMessageAsync(qualityProblemRecord.UserId, QualityMessageType.ImproveMessageNotPass, qualityProblemRecord.CreatorId);
            }
            else if (qualityProblemRecord.State == QualityRecordState.Passed)
            {
                qualityProblem.State = QualityProblemState.Improved;
                //await SendMessageAsync(qualityProblemRecord.UserId, QualityMessageType.ImproveMessagePassed, qualityProblemRecord.CreatorId);
            }

            await _recordRepository.InsertAsync(qualityProblemRecord);

            await _problemsRepository.UpdateAsync(qualityProblem);

            return ObjectMapper.Map<QualityProblemRecord, QualityProblemRecordDto>(qualityProblemRecord);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Stream> Export(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var problem = _problemsRepository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (problem == null) throw new UserFriendlyException("该质量问题不存在");

            var problemRecords = _recordRepository.Where(x => x.QualityProblemId == id).ToList();

            string fileName = "质量问题报告明细";

            string tableName = "整改验证记录";

            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();

            //文本标题
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, fileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 10, "宋体", ParagraphAlignment.CENTER), 1);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 10, "宋体", ParagraphAlignment.CENTER), 2);

            //TODO:这里一行需要显示两个文本
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"问题标题：{problem.Title}", false, 10, "宋体", ParagraphAlignment.LEFT, true, $"       问题类型：{GetProblemTypeName(problem.Type)}"), 3);
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
                xwpfTable.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{(item.Type == QualityRecordType.Improve ? "整改" : "验证")}", ParagraphAlignment.CENTER, 22, false));
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

        ///// <summary>
        ///// 更新
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public Task<QualityProblemDto> Update(QualityProblemUpdateDto input)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            await _problemsRepository.DeleteAsync(id);

            return true;
        }

        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task SendMessageAsync(Guid? userId, QualityMessageType type, Guid? creatorId)
        {
            var message = new NoticeMessage();
            message.SetUserId(userId.GetValueOrDefault());// 配置接收此消息的人员id
            var content = "";
            if (type == QualityMessageType.ImproveMessage)
            {
                content = "您有要整改的质量问题！";
            }
            else if (type == QualityMessageType.VerifyMessage)
            {
                content = "您有要验证的质量问题！";
            }
            else if (type == QualityMessageType.ImproveMessageNotPass)
            {
                content = "您提交的质量问题整改未通过！";
            }
            else if (type == QualityMessageType.ImproveMessagePassed)
            {
                content = "您提交的质量问题整改已通过！";
            }
            var messagepData = new NoticeMessageContent
            {
                Content = JsonConvert.SerializeObject(new
                {
                    Content = content,
                }),
                SponsorId = creatorId.GetValueOrDefault()
            };
            // 添加消息内容
            message.SetContent(messagepData);
            ////调用接口，发送消息,发送时需要调用GetBinary方法，将消息转换成二进制数据
            await _messageNotice.PushAsync(message.GetBinary());
        }

        /// <summary>
        /// 获取问题类型内容
        /// </summary>
        /// <returns></returns>
        private string GetProblemTypeName(QualityProblemType Type)
        {
            string typeName = null;

            if (Type == QualityProblemType.A)
            {
                typeName = "A类";
            }
            if (Type == QualityProblemType.B)
            {
                typeName = "B类";
            }
            if (Type == QualityProblemType.C)
            {
                typeName = "C类";
            }
            return typeName;
        }

    }
}
