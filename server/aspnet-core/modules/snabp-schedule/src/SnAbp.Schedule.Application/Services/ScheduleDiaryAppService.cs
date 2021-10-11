using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnAbp.Schedule.Dtos;
using SnAbp.Schedule.Entities;
using SnAbp.Schedule.Enums;
using SnAbp.Schedule.IServices;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Schedule.Services
{
    public class ScheduleDiaryAppService : ScheduleAppService, IScheduleDiaryAppService
    {
        private readonly IRepository<DiaryRltMaterial, Guid> _diaryRltMaterialRepository;
        private readonly IGuidGenerator _guidGenerate;
        private readonly IRepository<Diary, Guid> _diaryRepository;
        private readonly IRepository<DiaryRltFile, Guid> _diaryRltFileRepository;
        private readonly IRepository<DiaryRltBuilder, Guid> _diaryRltBuilderRepository;
        private readonly IRepository<Approval, Guid> _approvalRepository;

        public ScheduleDiaryAppService(
        #region 仓储
           IGuidGenerator guidGenerate,
           IRepository<Diary, Guid> diaryRepository,
           IRepository<DiaryRltFile, Guid> diaryRltFileRepository,
           IRepository<DiaryRltMaterial, Guid> diaryRltMaterialRepository,
           IRepository<DiaryRltBuilder, Guid> diaryRltBuilderRepository,
           IRepository<Approval, Guid> approvalRepository

            )
        {
            _diaryRltMaterialRepository = diaryRltMaterialRepository;
            _guidGenerate = guidGenerate;
            _diaryRepository = diaryRepository;
            _diaryRltFileRepository = diaryRltFileRepository;
            _diaryRltBuilderRepository = diaryRltBuilderRepository;
            _approvalRepository = approvalRepository;
        }
        #endregion
        #region 获取单个日志记录信息
        public Task<DiaryDto> Get(DiarySimpleDto input)
        {
            var diary = _diaryRepository.WithDetails()
                .WhereIf(input.DiaryId != null && input.DiaryId != Guid.Empty, x => x.Id == input.DiaryId)
                 .WhereIf(input.ApprovalId != null && input.ApprovalId != Guid.Empty, x => x.ApprovalId == input.ApprovalId)
                .FirstOrDefault();
            if (diary == null) throw new UserFriendlyException("此日志记录不存在");
            var diaryDto = ObjectMapper.Map<Diary, DiaryDto>(diary);
            foreach (var item in diaryDto.DiaryRltFiles)
            {
                if (item.Type == DiaryRltFileType.ProcessMedias)
                {
                    diaryDto.ProcessMedias.Add(item);
                }
                if (item.Type == DiaryRltFileType.TalkMedias)
                {
                    diaryDto.TalkMedias.Add(item);
                }
                if (item.Type == DiaryRltFileType.Pictures)
                {
                    diaryDto.Pictures.Add(item);
                }
            }
            foreach (var item in diaryDto.DirectorsRltBuilders)
            {
                if (item.Type == DiaryRltBuilderType.Director)
                {
                    diaryDto.Directors.Add(item);
                }
                if (item.Type == DiaryRltBuilderType.Builder)
                {
                    diaryDto.Builders.Add(item);
                }
            }
            foreach (var item in diaryDto.DiaryRltMaterials)
            {

                if (item.Type == MaterialsType.Appliance)
                {
                    diaryDto.ApplianceList.Add(item);
                }
                if (item.Type == MaterialsType.Mechanical)
                {
                    diaryDto.MechanicalList.Add(item);
                }
                if (item.Type == MaterialsType.AutoCompute)
                {
                    diaryDto.MaterialList.Add(item);
                }
                if (item.Type == MaterialsType.SafetyArticle)
                {
                    diaryDto.SecurityProtectionList.Add(item);
                }
            }
            return Task.FromResult(diaryDto);
        }
        #endregion
        #region 获取提交的施工日志信息
        public Task<PagedResultDto<ApprovalMultipleDto>> GetList(ApprovalSearchDto input)
        {
            var result = new PagedResultDto<ApprovalMultipleDto>();
            var approval = _approvalRepository.WithDetails()
                .Where(x => x.State == StatusType.Pass)
                 .WhereIf(input.ProfessionId != Guid.Empty && input.ProfessionId != null, x => x.ProfessionId == input.ProfessionId)
                 .WhereIf(input.StartTime != null && input.EndTime != null, x => x.Time >= input.StartTime && x.Time <= input.EndTime)
                 .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords) || x.Location.Contains(input.Keywords));
            //获得我填报日志的id
            if (input.IsCreator == true)
            {
                var diary_creatorIds = _diaryRepository.WithDetails().Where(x => x.CreatorId == CurrentUser.Id).Select(x => x.ApprovalId).ToList();
                approval = approval.Where(x => diary_creatorIds.Contains(x.Id));
            }
            result.TotalCount = approval.Count();
            var approvalDto = ObjectMapper.Map<List<Approval>, List<ApprovalMultipleDto>>(approval.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            //2、需要获取填报日志的编号和填报时间
            foreach (var item in approvalDto)
            {
                var diary = _diaryRepository.WithDetails().Where(x => x.ApprovalId == item.Id).FirstOrDefault();
                if (diary != null)
                {
                    item.DiaryCode = diary.Code;
                    item.FillTime = diary.FillTime;
                }

            }

            result.Items = approvalDto;
            return Task.FromResult(result);
        }
        #endregion
        #region 获取日志统计
        /// <summary>
        /// 获取日志统计消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<DiaryLogStatisticsDto>> GetLogStatistics(DiaryLogStatisticsSearchDto input)
        {
            var diaryLog = new List<DiaryLogStatisticsDto>();
            var diary = _diaryRepository.WithDetails()
                .Where(x => x.CreatorId == CurrentUser.Id)
              .WhereIf(input.StartTime != null && input.EndTime != null, x => x.FillTime >= input.StartTime && x.FillTime <= input.EndTime);

            for (var date = input.StartTime; date <= input.EndTime; date = date.AddMonths(1))
            {
                var monthDiaryLog = new DiaryLogStatisticsDto();
                monthDiaryLog.DateTime = date;
                monthDiaryLog.DiaryLogDayStatisticsDto = new List<DiaryLogDayStatisticsDto>();
                // 得到某一年某一月的日志记录
                var _diary = diary.Where(x => x.FillTime.Year == date.Year && x.FillTime.Month == date.Month).ToList();
                var days = DateTime.DaysInMonth(date.Year, date.Month);
                monthDiaryLog.SumDay = days;
                var _sumLog = 0;
                for (var j = 1; j <= days; j++)
                {
                    var diaryLogDayStatisticsDto = new DiaryLogDayStatisticsDto();
                    diaryLogDayStatisticsDto.Date = new DateTime(date.Year, date.Month, j);
                    if (_diary.Where(x => x.FillTime.Day == diaryLogDayStatisticsDto.Date.Day).Count() > 0)
                    {
                        diaryLogDayStatisticsDto.HasDiary = true;
                        _sumLog += 1;
                    }
                    else
                    {
                        diaryLogDayStatisticsDto.HasDiary = false;
                    }

                    monthDiaryLog.DiaryLogDayStatisticsDto.Add(diaryLogDayStatisticsDto);
                }
                monthDiaryLog.SumLog = _sumLog;
                diaryLog.Add(monthDiaryLog);
            }

            return Task.FromResult(diaryLog);
        }
        #endregion
        #region 增加
        public async Task<DiaryDto> Create(DiaryCreateDto input)
        {
            var diary = new Diary();
            ObjectMapper.Map(input, diary);
            //1、保存基本信息
            diary.SetId(_guidGenerate.Create());
            //2、保存负责人+施工员
            diary.DirectorsRltBuilders = new List<DiaryRltBuilder>();
            foreach (var item in input.Builders)
            {
                diary.DirectorsRltBuilders.Add(
                    new DiaryRltBuilder(_guidGenerate.Create())
                    {
                        BuilderId = item.BuilderId,
                        Type = DiaryRltBuilderType.Builder,
                    });
            }
            foreach (var item in input.Directors)
            {
                diary.DirectorsRltBuilders.Add(
                    new DiaryRltBuilder(_guidGenerate.Create())
                    {
                        BuilderId = item.BuilderId,
                        Type = DiaryRltBuilderType.Director,
                    });
            }
            //3.保存文件
            diary.DiaryRltFiles = new List<DiaryRltFile>();
            foreach (var item in input.ProcessMedias)
            {
                diary.DiaryRltFiles.Add(
                    new DiaryRltFile(_guidGenerate.Create())
                    {
                        FileId = item.FileId,
                        Type = DiaryRltFileType.ProcessMedias,
                    });
            }
            foreach (var item in input.TalkMedias)
            {
                diary.DiaryRltFiles.Add(
                    new DiaryRltFile(_guidGenerate.Create())
                    {
                        FileId = item.FileId,
                        Type = DiaryRltFileType.TalkMedias,
                    });
            }
            foreach (var item in input.Pictures)
            {
                diary.DiaryRltFiles.Add(
                    new DiaryRltFile(_guidGenerate.Create())
                    {
                        FileId = item.FileId,
                        Type = DiaryRltFileType.Pictures,
                    });
            }
            // 4、保存物资信息
            diary.DiaryRltMaterials = new List<DiaryRltMaterial>();
            foreach (var materialList in input.MaterialList)
            {
                diary.DiaryRltMaterials.Add(new DiaryRltMaterial(_guidGenerate.Create())
                {
                    MaterialName = materialList.MaterialName,
                    SpecModel = materialList.SpecModel,
                    Unit = materialList.Unit,
                    Number = materialList.Number,
                    Type = MaterialsType.AutoCompute,
                });
            }
            foreach (var mechanicalList in input.MechanicalList)
            {
                diary.DiaryRltMaterials.Add(new DiaryRltMaterial(_guidGenerate.Create())
                {
                    MaterialName = mechanicalList.MaterialName,
                    SpecModel = mechanicalList.SpecModel,
                    Unit = mechanicalList.Unit,
                    Number = mechanicalList.Number,
                    Type = MaterialsType.Mechanical,
                });
            }
            foreach (var applianceList in input.ApplianceList)
            {
                diary.DiaryRltMaterials.Add(new DiaryRltMaterial(_guidGenerate.Create())
                {
                    MaterialName = applianceList.MaterialName,
                    SpecModel = applianceList.SpecModel,
                    Unit = applianceList.Unit,
                    Number = applianceList.Number,
                    Type = MaterialsType.Appliance,
                });
            }
            foreach (var securityProtectionList in input.SecurityProtectionList)
            {
                diary.DiaryRltMaterials.Add(new DiaryRltMaterial(_guidGenerate.Create())
                {
                    MaterialName = securityProtectionList.MaterialName,
                    SpecModel = securityProtectionList.SpecModel,
                    Unit = securityProtectionList.Unit,
                    Number = securityProtectionList.Number,
                    Type = MaterialsType.SafetyArticle,
                });
            }
            await _diaryRepository.InsertAsync(diary);
            var diaryDto = ObjectMapper.Map<Diary, DiaryDto>(diary);
            return diaryDto;
        }
        #endregion
        #region 修改
        public async Task<DiaryDto> Update(DiaryUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的日志");
            var diary = await _diaryRepository.GetAsync(input.Id);
            if (diary == null) throw new UserFriendlyException("当前日志不存在");
            //1、清除保存的施工人员和负责人信息
            await _diaryRltBuilderRepository.DeleteAsync(x => x.DiaryId == input.Id);
            //2、清除保存的文件信息
            await _diaryRltFileRepository.DeleteAsync(x => x.DiaryId == input.Id);
            //3、清除保存的物资信息
            await _diaryRltMaterialRepository.DeleteAsync(x => x.DiaryId == input.Id);
            // 4、保存基本信息
            ObjectMapper.Map(input, diary);
            //5、保存负责人+施工员
            diary.DirectorsRltBuilders = new List<DiaryRltBuilder>();
            foreach (var item in input.Builders)
            {
                diary.DirectorsRltBuilders.Add(
                    new DiaryRltBuilder(_guidGenerate.Create())
                    {
                        BuilderId = item.BuilderId,
                        Type = DiaryRltBuilderType.Builder,
                    });
            }
            foreach (var item in input.Directors)
            {
                diary.DirectorsRltBuilders.Add(
                    new DiaryRltBuilder(_guidGenerate.Create())
                    {
                        BuilderId = item.BuilderId,
                        Type = DiaryRltBuilderType.Director,
                    });
            }
            //6、保存文件
            diary.DiaryRltFiles = new List<DiaryRltFile>();
            foreach (var item in input.ProcessMedias)
            {
                diary.DiaryRltFiles.Add(
                    new DiaryRltFile(_guidGenerate.Create())
                    {
                        FileId = item.FileId,
                        Type = DiaryRltFileType.ProcessMedias,
                    });
            }
            foreach (var item in input.TalkMedias)
            {
                diary.DiaryRltFiles.Add(
                    new DiaryRltFile(_guidGenerate.Create())
                    {
                        FileId = item.FileId,
                        Type = DiaryRltFileType.TalkMedias,
                    });
            }
            foreach (var item in input.Pictures)
            {
                diary.DiaryRltFiles.Add(
                    new DiaryRltFile(_guidGenerate.Create())
                    {
                        FileId = item.FileId,
                        Type = DiaryRltFileType.Pictures,
                    });
            }
            // 7、保存物资信息
            diary.DiaryRltMaterials = new List<DiaryRltMaterial>();
            foreach (var materialList in input.MaterialList)
            {
                diary.DiaryRltMaterials.Add(new DiaryRltMaterial(_guidGenerate.Create())
                {
                    MaterialName = materialList.MaterialName,
                    SpecModel = materialList.SpecModel,
                    Unit = materialList.Unit,
                    Number = materialList.Number,
                    Type = MaterialsType.AutoCompute,
                });
            }
            foreach (var mechanicalList in input.MechanicalList)
            {
                diary.DiaryRltMaterials.Add(new DiaryRltMaterial(_guidGenerate.Create())
                {
                    MaterialName = mechanicalList.MaterialName,
                    SpecModel = mechanicalList.SpecModel,
                    Unit = mechanicalList.Unit,
                    Number = mechanicalList.Number,
                    Type = MaterialsType.Mechanical,
                });
            }
            foreach (var applianceList in input.ApplianceList)
            {
                diary.DiaryRltMaterials.Add(new DiaryRltMaterial(_guidGenerate.Create())
                {
                    MaterialName = applianceList.MaterialName,
                    SpecModel = applianceList.SpecModel,
                    Unit = applianceList.Unit,
                    Number = applianceList.Number,
                    Type = MaterialsType.Appliance,
                });
            }
            foreach (var securityProtectionList in input.SecurityProtectionList)
            {
                diary.DiaryRltMaterials.Add(new DiaryRltMaterial(_guidGenerate.Create())
                {
                    MaterialName = securityProtectionList.MaterialName,
                    SpecModel = securityProtectionList.SpecModel,
                    Unit = securityProtectionList.Unit,
                    Number = securityProtectionList.Number,
                    Type = MaterialsType.SafetyArticle,
                });
            }
            await _diaryRepository.UpdateAsync(diary);
            var diaryDto = ObjectMapper.Map<Diary, DiaryDto>(diary);
            return diaryDto;
        }
        #endregion
        #region 删除
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _diaryRepository.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此日志不存在");
            await _diaryRepository.DeleteAsync(id);
            return true;
        }

        #endregion

        #region  导出
        [Produces("application/octet-stream")]
        public Task<Stream> Export(DiaryExportSearchDto input)
        {
            Stream stream = null;
            byte[] sbuf;
            var dataTable = (DataTable)null;//表
            var dataColumn = (DataColumn)null;//行
            var dataRow = (DataRow)null;//列
                                        //获取需要导出的数据

            var diary = getListDiary(input);
            if (diary.Count == 0) throw new UserFriendlyException("未找到任何导出数据");
            //表格初始化
            dataTable = new DataTable();
            var enumValues = Enum.GetValues(typeof(Enums.DiaryExcelCol));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    dataColumn = new DataColumn(Enum.GetName(typeof(Enums.DiaryExcelCol), item));
                    dataTable.Columns.Add(dataColumn);
                }
            }
            var index = 0;
            //表格添加数据
            foreach (var item in diary)
            {
                dataRow = dataTable.NewRow();
                dataRow[DiaryExcelCol.序号.ToString()] = index;
                dataRow[DiaryExcelCol.填报时间.ToString()] = item.DiaryCode != null ? string.Format("{0:d}", item.FillTime) : "";
                dataRow[DiaryExcelCol.备注.ToString()] = item.Remark;

                dataRow[DiaryExcelCol.存在的问题.ToString()] = item.Problem;
                dataRow[DiaryExcelCol.计划劳务人员.ToString()] = item.MemberNum;
                dataRow[DiaryExcelCol.实际劳务人员.ToString()] = item.DiaryCode != null ? item.ReaLMemberNum.ToString() : "";
                dataRow[DiaryExcelCol.施工专业.ToString()] = item.Profession.Name;
                dataRow[DiaryExcelCol.施工内容.ToString()] = item.Name;
                dataRow[DiaryExcelCol.施工单位.ToString()] = item.Organization;
                dataRow[DiaryExcelCol.施工描述.ToString()] = item.Discription;
                dataRow[DiaryExcelCol.施工日期.ToString()] = string.Format("{0:d}", item.Time);
                dataRow[DiaryExcelCol.施工部位.ToString()] = item.Location;
                dataRow[DiaryExcelCol.日志编号.ToString()] = item.DiaryCode;
                dataRow[DiaryExcelCol.温度.ToString()] = item.Temperature;


                var directorName = "";
                var builderName = "";
                if (item.DirectorsRltBuilders.Count > 0)
                {
                    foreach (var user in item.DirectorsRltBuilders)
                    {
                        if (user.Type == DiaryRltBuilderType.Director)
                        {
                            directorName += directorName == "" ? user.Builder.Name : "、" + directorName;
                        }
                        if (user.Type == DiaryRltBuilderType.Builder)
                        {
                            builderName += builderName == "" ? user.Builder.Name : "、" + builderName;
                        }

                    }
                }
                var weather = "";
                if (item.Weathers != null)
                {
                    JObject obj = (JObject)JsonConvert.DeserializeObject(item.Weathers);
                    weather = "上午：" + obj["morning_t"].ToString() + ",下午：" + obj["afternoon_t"].ToString() + ",晚上：" + obj["evening_t"].ToString();
                }

                dataRow[DiaryExcelCol.天气.ToString()] = weather;
                dataRow[DiaryExcelCol.现场负责人.ToString()] = directorName;
                dataRow[DiaryExcelCol.施工员.ToString()] = builderName;
                dataRow[DiaryExcelCol.状态.ToString()] = item.DiaryCode == null ? "未填报" : "已填报";
                dataTable.Rows.Add(dataRow);
                index++;
            }
            sbuf = ExcelHelper.DataTableToExcel(dataTable, "项目工作汇报表.xlsx", null, "项目工作汇报记录");
            stream = new MemoryStream(sbuf);
            return Task.FromResult(stream);
        }

        #endregion
        #region 获取提交的施工日志信息
        List<DiaryExportDto> getListDiary(DiaryExportSearchDto input)
        {
            var result = new List<ApprovalMultipleDto>();
            var approval = _approvalRepository.WithDetails()
                .Where(x => x.State == StatusType.Pass)
                .WhereIf(input.Ids.Count > 0, x => input.Ids.Contains(x.Id))
                 .WhereIf(input.ProfessionId != Guid.Empty && input.ProfessionId != null, x => x.ProfessionId == input.ProfessionId)
                 .WhereIf(input.StartTime != null && input.EndTime != null, x => x.Time >= input.StartTime && x.Time <= input.EndTime)
                 .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords) || x.Location.Contains(input.Keywords));
            //获得我填报日志的id
            if (input.IsCreator == true)
            {
                var diary_creatorIds = _diaryRepository.WithDetails().Where(x => x.CreatorId == CurrentUser.Id).Select(x => x.ApprovalId).ToList();
                approval = approval.Where(x => diary_creatorIds.Contains(x.Id));
            }
            var approvalDto = ObjectMapper.Map<List<Approval>, List<DiaryExportDto>>(approval.OrderByDescending(x => x.CreationTime).ToList());
            //2、需要获取填报日志的编号和填报时间
            foreach (var item in approvalDto)
            {
                var diary = _diaryRepository.WithDetails().Where(x => x.ApprovalId == item.Id).FirstOrDefault();
                var diaryDto = ObjectMapper.Map<Diary, DiaryDto>(diary);
                if (diary != null)
                {
                    item.DiaryCode = diaryDto.Code;
                    item.FillTime = diaryDto.FillTime;
                    item.Problem = diaryDto.Problem;
                    item.Temperature = diaryDto.Temperature;
                    item.Discription = diaryDto.Discription;
                    item.Weathers = diaryDto.Weathers;
                    item.ReaLMemberNum = diaryDto.MemberNum;
                    item.DirectorsRltBuilders = diaryDto.DirectorsRltBuilders;

                }

            }

            return approvalDto;
        }

        public Task<PagedResultDto<SpeachVideoDto>> GetSpeachVideo(SpeachSearchDto input)
        {
            /*获取施工日志中的班前讲话视频*/
            var result = new PagedResultDto<SpeachVideoDto>();
            
            var dto = new List<SpeachVideoDto>();
            var approval = _diaryRepository.WithDetails()
                .WhereIf(input.StartTime != null, x => x.FillTime >= input.StartTime)
                .WhereIf(input.EndTime != null, x => x.FillTime <= input.EndTime)
                .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Approval.Location.Contains(input.Keywords) || x.Approval.Name.Contains(input.Keywords));
            var approvalList = approval.OrderByDescending(x => x.FillTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            foreach (var item in approvalList)
            {
                var speachVideoDto = new SpeachVideoDto();
                speachVideoDto.Id = item.Id;
                speachVideoDto.Location = item.Approval.Location;
                speachVideoDto.Date = item.FillTime;
                speachVideoDto.Schedule = item.Approval.Name;
                foreach(var files in item.DiaryRltFiles)
                {
                    if(files.Type== DiaryRltFileType.TalkMedias)
                    {
                        speachVideoDto.TalkMedias.Add(files);
                    }
                }
                
                dto.Add(speachVideoDto);
            }
            result.Items = dto;
            result.TotalCount = approval.Count();
            return Task.FromResult(result);
        }
        #endregion
    }
}
